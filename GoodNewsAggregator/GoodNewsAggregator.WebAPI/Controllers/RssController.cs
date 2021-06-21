using GoodNewsAggregator.Core.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using System;
using System.Threading.Tasks;

namespace GoodNewsAggregator.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RssController : ControllerBase
    {
        private readonly IRSSService _rssService;

        public RssController(IRSSService rssService)
        {
            _rssService = rssService;
        }

        /// <summary>
        /// Get RSS by rss id. Admin.
        /// </summary>
        /// <param name="id"></param> 
        [HttpGet("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Get(Guid id)
        {
            try
            {
                var rss = await _rssService.FindRssById(id);
                if (rss != null)
                {
                    return Ok(rss);
                }
                else
                {
                    return NotFound();
                }
            }
            catch (Exception e)
            {
                Log.Error(e, "RssController Get was not successful");
                throw;
            }            
        }

        /// <summary>
        /// Get all RSSes. Admin.
        /// </summary>
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Get()
        {
            try
            {
                var rsses = await _rssService.FindRss();
                return Ok(rsses);
            }
            catch (Exception e)
            {
                Log.Error(e, "RssController Get was not successful");
                throw;
            }            
        }

        //[HttpPatch("{id}")]
        //public async Task<IActionResult> Patch(int id, [FromBody] RSSDto value)
        //{
        //    //Send only properties of the model that should be updated 
        //}
    }
}
