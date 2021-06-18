using GoodNewsAggregator.Core.DataTransferObjects;
using GoodNewsAggregator.Core.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
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

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid? id)
        {
            if (id == null)
            {
                return BadRequest();
            }
            else
            {
                var news = await _newsService.GetNewsWithRSSAddressById(id);
                if (news !=null)
                {
                    return Ok(news);
                }
                else
                {
                    return NotFound();
                }               
            }
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var news = await _newsService.GetNewsWithRSSAddressById(null);
            return Ok(news);
        }

        [HttpPost]
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
            catch (Exception)
            {
                return BadRequest();
            }
        }
    }
}
