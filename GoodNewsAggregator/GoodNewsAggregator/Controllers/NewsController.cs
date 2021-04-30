﻿using System;
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
using System.Xml;
using GoodNewsAggregator.Core.DataTransferObjects;
using Serilog;
using Serilog.Events;
using GoodNewsAggregator;
using GoodNewsAggregator.Filters;
using GoodNewsAggregator.Services.Implementation;
using GoodNewsAggregator.Models.ViewModels;

namespace GoodNewsAggregator.Controllers
{
    public class NewsController : Controller
    {
        private readonly INewsService _newsService;
        private readonly IRSSService _rssService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly OnlinerParser _onlinerParser;
        private readonly TutByParser _tutByParser;
        private readonly S13Parser _s13Parser;

        public NewsController(INewsService newsService, IRSSService rssService, OnlinerParser onlinerParser, TutByParser tutByParser, S13Parser s13Parser, IUnitOfWork unitOfWork)
        {
            _newsService = newsService;
            _rssService = rssService;
            _onlinerParser = onlinerParser;
            _tutByParser = tutByParser;
            _s13Parser = s13Parser;
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        public async Task<IActionResult> Aggregate()
        {
            return View();
        }

        //POST: News/Aggregate
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Aggregate(CreateNewsViewModel sourse)
        {
            try
            {
                var stopwatch = new Stopwatch();
                stopwatch.Start();
                var rssSourses = await _rssService
                    .FindRss();

                List<RSSDto> listOfRssDtos = rssSourses.ToList();

                var newsInfos = new List<NewsDto>(); // without any duplicate

                List<NewsDto> allNews = new List<NewsDto>();

                foreach (var rss in listOfRssDtos)
                {
                    allNews.AddRange(_newsService.GetNewsInfoFromRssSourse(rss).Result);
                }

                Parallel.ForEach(allNews, news =>
                {
                    if (news.RSS_Id.Equals(new Guid("f68ffbd2-3ae5-4e80-be43-6021233c6ec9")))
                    {
                            var newsBody = _tutByParser.Parse(news.Address);
                            news.Text = newsBody.Result;
                    }
                    else if (news.RSS_Id.Equals(new Guid("b637e8d9-dc89-4feb-951c-23d3baa7c48d")))
                    {
                            var newsBody = _s13Parser.Parse(news.Address);
                            news.Text = newsBody.Result;
                    }
                    else if (news.RSS_Id.Equals(new Guid("972036b6-175f-4251-b2d9-296a77b65169")))
                    {
                            var newsBody = _onlinerParser.Parse(news.Address);
                            news.Text = newsBody.Result;
                    }                   
                });
                newsInfos.AddRange(allNews);                

                await _newsService.AddRangeNews(newsInfos);
                
                stopwatch.Stop();
                Log.Information($"Aggregation was executed in {stopwatch.ElapsedMilliseconds}");
            }
            catch (Exception e)
            {
                Log.Error(e, $"{e.Message}");
            }
            return RedirectToAction(nameof(AllNews));
        }

        //GET: News/Create
        public async Task<IActionResult> Create()
        {

            var model = new CreateNewsViewModel()
            {
                Sources = new SelectList(await _rssService.FindRss(),
                    "Id", //field of element with value
                    "Address") //field of element with text
            };
            return View(model);
        }

        //// GET: News/AllNews
        //[HttpGet]
        //public async Task<IActionResult> AllNews()
        //{
        //    var newsDtos = await _newsService.FindNews();

        //    var newsViewModels = newsDtos.Select(n => new NewsViewModel()
        //    {
        //        Id = n.Id,
        //        Address = n.Address,
        //        Description = n.Description,
        //        GoodnessCoefficient = n.GoodnessCoefficient,
        //        PublicationDate = n.PublicationDate,
        //        RSS_Id = n.RSS_Id,
        //        Text = n.Text,
        //        Title = n.Title
        //    }).ToList();

        //    return View(newsViewModels);
        //}

        // GET: News/AllNews
        [HttpGet]
        public async Task<IActionResult> AllNews(int page = 1)
        {
            try
            {              
                var news = await _newsService.FindNews();

                var pageSize = 12;

                var newsPerPages = news.Skip((page - 1) * pageSize).Take(pageSize);

                var pageInfo = new PageInfo()
                {
                    PageNumber = page,
                    PageSize = pageSize,
                    TotalItems = news.Count()
                };

                return View(new NewsListWithPaginationInfo()
                {
                    News = newsPerPages,
                    PageInfo = pageInfo
                });
            }
            catch (Exception e)
            {
                Log.Error(e, $"Unhandled exception was thrown by app");
                throw;
            }
        }


        // GET: News/SingleNews
        [HttpGet]
        public async Task<IActionResult> SingleNews(Guid? id)
        {
            try
            {
                if (id == null)
                {
                    return NotFound();
                }
                else
                {
                    var newsDto = await _newsService.GetNewsById(id);

                    var rSSDto = await _rssService.FindRssById(newsDto.RSS_Id);

                    var newsWithRSSAddressViewModel = new NewsWithRSSAddressViewModel()
                    {
                        Id = newsDto.Id,
                        Title = newsDto.Title,
                        Address = newsDto.Address,
                        Description = newsDto.Description,
                        PublicationDate = newsDto.PublicationDate,
                        Text = newsDto.Text,
                        GoodnessCoefficient = newsDto.GoodnessCoefficient,

                        RSS_Id = rSSDto.Id,
                        RSSAddress = rSSDto.Address
                    };

                    return View(newsWithRSSAddressViewModel);
                }
            }
            catch (Exception e)
            {
                Log.Error(e, $"Unhandled exception was thrown by app");
                throw;
            }
        }

        // GET: News/Edit
        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
            try
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
            catch (Exception e)
            {
                Log.Error(e, $"Unhandled exception was thrown by app");
                throw;
            }
        }

        // POST: News/Edit
        [HttpPatch]
        public IActionResult Edit()
        {
            try
            {
                return RedirectToAction(nameof(AllNews));
            }
            catch (Exception e)
            {
                Log.Error(e, $"Unhandled exception was thrown by app");
                throw;
            }
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            try
            {
                return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
            }
            catch (Exception e)
            {
                Log.Error(e, $"Unhandled exception was thrown by app");
                throw;
            }
        }

        //[ServiceFilter(typeof(CheckDataFilterAttribute))]
        //public IActionResult Privacy1(int hiddenId)
        //{
        //    return View("Privacy");
        //}
    }
}
