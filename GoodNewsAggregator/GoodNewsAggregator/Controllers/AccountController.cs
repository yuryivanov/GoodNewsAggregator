using GoodNewsAggregator.Core.DataTransferObjects;
using GoodNewsAggregator.Core.Services.Interfaces;
using GoodNewsAggregator.Models.ViewModels.Account;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace GoodNewsAggregator.Controllers
{
    public class AccountController : Controller
    {
        private readonly IUserService _userService;
        private readonly IRoleService _roleService;

        public AccountController(IUserService userService, IRoleService roleService)
        {
            _userService = userService;
            _roleService = roleService;
        }

        //TODO create a link back to news from login form

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {           
            if (ModelState.IsValid)
            {
                var passwordHash = _userService.GetPasswordHash(model.Password);

                var result = await _userService.RegisterUser(new UserDto()
                {
                    Id = new Guid(),
                    PasswordHash = passwordHash,
                    Email = model.Email,
                    FullName = model.FullName
                });

                //if (result)
                //{
                return RedirectToAction("AllNews", "News");
                //}
                //return BadRequest();
            }
            return View(model);
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var userFromDb = await _userService.GetUserByEmail(model.Email);
                if (userFromDb != null)
                {
                    var passwordHash = _userService.GetPasswordHash(model.Password);
                    if (passwordHash.Equals(userFromDb.PasswordHash))
                    {
                        await Authenticate(userFromDb);

                        return string.IsNullOrEmpty(model.ReturnUrl)
                            ? (IActionResult)RedirectToAction("AllNews", "News")
                            : Redirect(model.ReturnUrl);
                    }
                }
            }
            return View(model);
        }

        private async Task Authenticate(UserDto dto)
        {
            try
            {
                const string authType = "ApplicationCookie";
                var claims = new List<Claim>
                {
                    new Claim(ClaimsIdentity.DefaultNameClaimType, dto.Email),
                    new Claim(ClaimsIdentity.DefaultRoleClaimType, (await _roleService.FindRoleById(dto.RoleId)).Name)
                };

                var identity = new ClaimsIdentity(claims,
                    authType,
                    ClaimsIdentity.DefaultNameClaimType,
                    ClaimsIdentity.DefaultRoleClaimType);

                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(identity));
            }
            catch (Exception e)
            {
                Log.Error(e, "Authentication failed");
            }
        }

        [HttpGet]
        public async Task<IActionResult> LogOut()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("AllNews", "News");
        }

        [AcceptVerbs("Post", "Get")]
        public async Task<IActionResult> CheckEmail(string email)
        {
            return await _userService.GetUserByEmail(email) != null
            ? Json(false)
                : Json(true);
        }
    }
}

