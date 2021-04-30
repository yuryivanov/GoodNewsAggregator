using GoodNewsAggregator.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace GoodNewsAggregator.Controllers
{
    public class HomeController : Controller
    {
        public HomeController()
        {
        }

        [HttpGet]
        public IActionResult Login()
        {
            
            return View();
        }

        [HttpPost]
        public IActionResult Login(string login, string password)
        {
            return View("~/Views/News/AllNews.cshtml");
        }
    }
}
