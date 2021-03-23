using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using GoodNewsAggregator.Models;
using System.Diagnostics;
using System.Linq;
using System.IO;
using GoodNewsAggregator.DAL.Core;
using GoodNewsAggregator.Core.Services.Interfaces;

namespace GoodNewsAggregator.Controllers
{
    public class NewsController : Controller
    {
        private readonly INewsService _newsService;

        public NewsController(INewsService newsService)
        {
            _newsService = newsService;
        }

        // GET: News/AllNews
        [HttpGet]
        public IActionResult AllNews()
        {
            return View();
        }        

        // GET: News/SingleNews
        [HttpGet]
        public IActionResult SingleNews()
        {
            return View();
        }

        // GET: News/Edit
        [HttpGet]
        public IActionResult Edit(Guid id)
        {
            string idd = Request.Query.FirstOrDefault(r => r.Key == "id").Value;
        
            return RedirectToAction(nameof(AllNews));            
        }

        // POST: News/Edit
        [HttpPatch]
        public IActionResult Edit()
        {
            return RedirectToAction(nameof(AllNews));
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
