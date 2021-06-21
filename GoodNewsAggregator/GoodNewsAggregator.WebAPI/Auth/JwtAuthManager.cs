using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Configuration;
using System.Text;
using GoodNewsAggregator.Core.Services.Interfaces;
using GoodNewsAggregator.Core.DataTransferObjects;
using Serilog;

namespace GoodNewsAggregator.WebAPI.Auth
{
    public class JwtAuthManager : IJwtAuthManager
    {
        private readonly IConfiguration _configuration;
        private readonly IUserService _userService;

        public JwtAuthManager(IConfiguration configuration, IUserService userService)
        {
            _configuration = configuration;
            _userService = userService;
        }

        public async Task<JwtAuthResult> GenerateTokens(string email, Claim[] claims)
        {
            try
            {
                var accessTokenLifeTime = Convert.ToDouble(_configuration.GetSection("Jwt")["AccessTokenLifeTime"]);
                var refreshTokenLifeTime = Convert.ToDouble(_configuration.GetSection("Jwt")["RefreshTokenLifeTime"]);

                //generate jwt token
                var jwtToken = new JwtSecurityToken("GoodNewsAggregator",
                    "GoodNewsAggregator",
                    claims,
                    expires: DateTime.Now.AddMinutes(accessTokenLifeTime), //from config should be
                                                                           //how to encode our key:
                    signingCredentials: new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"])),
                    SecurityAlgorithms.HmacSha256Signature));

                //generate string from jwt token
                var accessToken = new JwtSecurityTokenHandler().WriteToken(jwtToken);

                var userId = _userService.GetUserByEmail(email).Result.Id;

                var refreshTokenDto = new RefreshTokenDto()
                {
                    Id = Guid.NewGuid(),
                    Email = email,
                    ExpireAt = DateTime.Now.AddMinutes(refreshTokenLifeTime), //from config
                    Token = Guid.NewGuid().ToString("D"),
                    //Format is aea7c9c5-ad89-481b-ba20-2bfda9ba19cb (lowercase with hyphens)
                    UserId = userId
                };

                await _userService.AddAccessToken(new AccessTokenDto()
                {
                    Id = Guid.NewGuid(),
                    ExpireAt = DateTime.Now.AddMinutes(accessTokenLifeTime),
                    Token = accessToken,
                    UserId = userId
                });

                await _userService.AddRefreshToken(refreshTokenDto);

                return new JwtAuthResult()
                {
                    AccessToken = accessToken,
                    RefreshTokenDtoId = refreshTokenDto.Id
                };
            }
            catch (Exception e)
            {
                Log.Error(e, "GenerateTokens was not successful");
                throw;
            }            
        }
    }

    public class JwtAuthResult
    {
        public string AccessToken { get; set; }
        public Guid RefreshTokenDtoId { get; set; }
    }

    public interface IJwtAuthManager
    {
        Task<JwtAuthResult> GenerateTokens(string email, Claim[] claims);
    }
}
