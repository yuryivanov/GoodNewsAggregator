using GoodNewsAggregator.Core.DataTransferObjects;
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
    public class CommentController : ControllerBase
    {
        private readonly ICommentService _commentService;

        public CommentController(ICommentService commentService)
        {
            _commentService = commentService;
        }

        /// <summary>
        /// Gets comments by news id. User.
        /// </summary>
        /// <param name="id"></param>
        [HttpGet("{id}")]
        [Authorize]
        public async Task<IActionResult> Get(Guid id)
        {
            try
            {
                var comments = await _commentService.FindCommentsByNewsId(id);
                if (comments != null)
                {
                    return Ok(comments);
                }
                else
                {
                    return Ok();
                }
            }
            catch (Exception e)
            {
                Log.Error(e, "CommentController Get was not successful");
                throw;
            }            
        }

        /// <summary>
        /// Add a comment. User.
        /// </summary>
        /// <param name="comment"></param>    
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Post(CommentDto comment)
        {
            try
            {
                var res = await _commentService.AddComment(comment);
                if (res)
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
                Log.Error(e, "CommentController Post was not successful");
                throw;
            }
        }
    }
}
