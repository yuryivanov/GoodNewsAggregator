using AutoMapper;
using GoodNewsAggregator.Core.DataTransferObjects;
using GoodNewsAggregator.Core.Services.Interfaces;
using GoodNewsAggregator.Models.ViewModels.Account;
using GoodNewsAggregator.WebAPI.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Serilog;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace GoodNewsAggregator.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TokenController : ControllerBase
    {
        private readonly IJwtAuthManager _jwtAuthManager;
        private readonly IUserService _userService;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;

        public TokenController(IJwtAuthManager jwtAuthManager, IUserService userService, IMapper mapper, IConfiguration configuration)
        {
            _jwtAuthManager = jwtAuthManager;
            _userService = userService;
            _mapper = mapper;
            _configuration = configuration;
        }

        /// <summary>
        /// Log in the application. Anonymous.
        /// </summary>
        /// <param name="request"></param> 
        [AllowAnonymous]
        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var userFromDb = _userService.GetUserByEmail(request.Email).Result;
                    var passwordHash = _userService.GetPasswordHash(request.Password);

                    JwtAuthResult jwtResult = null;

                    if (userFromDb != null)
                    {
                        if (request.Email == "admin@gmail.com")
                        {
                            if (passwordHash.Equals(userFromDb.PasswordHash))
                            {
                                var claims = new[]
                                {
                          new Claim(ClaimTypes.Email, request.Email),
                          new Claim(ClaimTypes.Role, "Admin")
                        };

                                jwtResult = await _jwtAuthManager.GenerateTokens(request.Email, claims);
                            }
                            else
                            {
                                return Unauthorized();
                            }
                        }
                        else if (passwordHash.Equals(userFromDb.PasswordHash))
                        {
                            var claims = new[]
                            {
                          new Claim(ClaimTypes.Email, request.Email),
                          new Claim(ClaimTypes.Role, "User")
                        };

                            jwtResult = await _jwtAuthManager.GenerateTokens(request.Email, claims);
                        }
                        else
                        {
                            return Unauthorized();
                        }
                    }
                    else
                    {
                        return Unauthorized();
                    }
                    return Ok(jwtResult);
                }
                else
                {
                    return Unauthorized();
                }
            }
            catch (Exception e)
            {
                Log.Error(e, "Login was not successful");
                throw;
            }            
        }

        /// <summary>
        /// Register in the application. Anonymous.
        /// </summary>
        /// <param name="request"></param> 
        [AllowAnonymous]
        [HttpPost]
        [Route("Register")]
        public async Task<IActionResult> Register([FromBody] RegisterViewModel request)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var passwordHash = _userService.GetPasswordHash(request.Password);

                    var result = await _userService.RegisterUser(new UserDto()
                    {
                        Id = Guid.NewGuid(),
                        PasswordHash = passwordHash,
                        Email = request.Email,
                        FullName = request.FullName,
                        RoleId = new Guid("07DEE73C-D974-46A7-AE2C-8367FCFBCB7F")
                    });

                    return Ok();
                }
                return BadRequest();
            }
            catch (Exception e)
            {
                Log.Error(e, "Register was not successful");
                throw;
            }            
        }

        /// <summary>
        /// Refresh tokens. Anonymous.
        /// </summary>
        /// <param name="jwtAuthResult"></param> 
        [AllowAnonymous]
        [HttpPost]
        [Route("RefreshToken")]
        public async Task<IActionResult> RefreshToken([FromBody] JwtAuthResult jwtAuthResult)
        {
            try
            {
                var accessTokenLifeTime = Convert.ToDouble(_configuration.GetSection("Jwt")["AccessTokenLifeTime"]);
                var refreshTokenLifeTime = Convert.ToDouble(_configuration.GetSection("Jwt")["RefreshTokenLifeTime"]);

                var refreshTokenDtoId = jwtAuthResult.RefreshTokenDtoId;
                var refreshTokenDto = _userService.GetRefreshTokenById(refreshTokenDtoId).Result;

                //This [Route()] is ok just for tokens in REST
                if (refreshTokenDto.ExpireAt > DateTime.Now)
                {
                    var userFromDb = _userService.GetUserByEmail(refreshTokenDto.Email).Result;

                    JwtAuthResult jwtResult = null;

                    Claim[] claims = null;

                    if (refreshTokenDto.Email == "admin@gmail.com")
                    {
                        claims = new[]
                        {
                          new Claim(ClaimTypes.Email, refreshTokenDto.Email),
                          new Claim(ClaimTypes.Role, "Admin")
                    };
                    }
                    else
                    {
                        claims = new[]
                        {
                          new Claim(ClaimTypes.Email, refreshTokenDto.Email),
                          new Claim(ClaimTypes.Role, "User")
                    };
                    }

                    jwtResult = await _jwtAuthManager.GenerateTokens(refreshTokenDto.Email, claims);

                    await _userService.AddAccessToken(new AccessTokenDto()
                    {
                        Id = Guid.NewGuid(),
                        ExpireAt = DateTime.Now.AddMinutes(accessTokenLifeTime),
                        Token = jwtResult.AccessToken,
                        UserId = userFromDb.Id
                    });

                    return Ok(jwtResult);
                }
                else
                {
                    return BadRequest();
                }
            }
            catch (Exception e)
            {
                Log.Error(e, "RefreshToken was not successful");
                throw;
            }            
        }
    }

    public class LoginRequest
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
