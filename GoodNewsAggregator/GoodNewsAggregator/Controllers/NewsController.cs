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
using GoodNewsAggregator.DAL.Repositories.Implementation;
using GoodNewsAggregator.Models.ViewModels.News;

namespace GoodNewsAggregator.Controllers
{
    public class NewsController : Controller
    {
        private readonly INewsService _newsService;
        private readonly IUnitOfWork _unitOfWork;

        public NewsController(INewsService newsService, IUnitOfWork unitOfWork)
        {
            _newsService = newsService;
            _unitOfWork = unitOfWork;
        }

        // GET: News/AllNews
        [HttpGet]
        public async Task<IActionResult> AllNews()
        {
            var newsDtos = await _newsService.FindNews();

            var newsViewModels = newsDtos.Select(n => new NewsViewModel()
            {
                Id = n.Id,
                Address = n.Address,
                Description = n.Description,
                GoodnessCoefficient = n.GoodnessCoefficient,
                PublicationDate = n.PublicationDate,
                RSS_Id = n.RSS_Id,
                Text = n.Text,
                Title = n.Title
            }).ToList();

            return View(newsViewModels);
        }        

        // GET: News/SingleNews
        [HttpGet]
        public async Task<IActionResult> SingleNews(Guid id)
        {
            if (id == null)
            {
                return NotFound();
            }
            else
            {
                var model = await _newsService.GetNewsById(id);
                return View(model);
            }            
        }

        // GET: News/Edit
        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
            if (id == null)
            {
                return NotFound();
            }
            else
            {
                var model = await _newsService.GetNewsById(id);
                return View(model);
            }
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
