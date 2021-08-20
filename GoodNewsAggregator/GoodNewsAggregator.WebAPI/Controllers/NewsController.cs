using GoodNewsAggregator.Core.DataTransferObjects;
using GoodNewsAggregator.Core.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GoodNewsAggregator.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NewsController : ControllerBase
    {
        private readonly INewsService _newsService;

        public NewsController(INewsService newsService)
        {
            _newsService = newsService;
        }

        /// <summary>
        /// Get news by news id. Anonymous.
        /// </summary>
        /// <param name="id"></param>    
        [AllowAnonymous]
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid? id)
        {
            try
            {
                if (id == null)
                {
                    return BadRequest();
                }
                else
                {
                    var news = await _newsService.GetNewsWithRSSAddressById(id);
                    if (news != null)
                    {
                        return Ok(news);
                    }
                    else
                    {
                        return NotFound();
                    }
                }
            }
            catch (Exception e)
            {
                Log.Error(e, "NewsController Get was not successful");
                throw;
            }            
        }

        /// <summary>
        /// Get all news. Anonymous.
        /// </summary>         
        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> Get()
        {              
            try
            {
                var news = await _newsService.GetNewsWithRSSAddressById(null);
                return Ok(news);
            }
            catch (Exception e)
            {
                Log.Error(e, "NewsController Get was not successful");
                throw;
            }            
        }

        /// <summary>
        /// Add news collection. Admin.
        /// </summary>
        /// <param name="news"></param>    
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Post(IEnumerable<NewsDto> news)
        {
            try
            {
                var res = await _newsService.AddRangeNews(news);
                if (res > 0)
                {
                    return Ok();
                }
                else
                {
                    return BadRequest();
                }
            }
            catch (Exception e)
            {
                Log.Error(e, "NewsController Post was not successful");
                throw;
            }
        }
    }
}
