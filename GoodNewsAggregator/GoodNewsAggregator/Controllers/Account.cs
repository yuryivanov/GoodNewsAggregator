using GoodNewsAggregator.Core.DataTransferObjects;
using GoodNewsAggregator.Core.Services.Interfaces;
using GoodNewsAggregator.Models.ViewModels.Account;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GoodNewsAggregator.Controllers
{
    public class Account : Controller
    {
        private readonly IUserService _userService;
        private readonly IRoleService _roleService;

        public Account(IUserService userService, IRoleService roleService)
        {
            _userService = userService;
            _roleService = roleService;
        }

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
        public IActionResult Login(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                return Ok(model);
            }

            return View();
        }

        [AcceptVerbs("Post","Get")]
        public async Task<IActionResult> CheckEmail(string email)
        {
            return await _userService.GetUserByEmail(email) != null
            ? Json(false)
                : Json(true);
        }
    }
}
