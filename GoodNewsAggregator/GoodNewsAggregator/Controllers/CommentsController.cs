using GoodNewsAggregator.Core.DataTransferObjects;
using GoodNewsAggregator.Core.Services.Interfaces;
using GoodNewsAggregator.Models.ViewModels.Comment;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using System;
using System.Threading.Tasks;

namespace GoodNewsAggregator.Controllers
{
    public class CommentsController : Controller
    {
        private readonly ICommentService _commentService;
        private readonly IUserService _userService;
        private readonly IRoleService _roleService;

        public CommentsController(ICommentService commentService, IUserService userService, IRoleService roleService)
        {
            _commentService = commentService;
            _userService = userService;
            _roleService = roleService;
        }

        public async Task<IActionResult> List(Guid newsId)
        {
            try
            {
                var comments = await _commentService.FindCommentsByNewsId(newsId);

                return View(new CommentsListViewModel
                {
                    NewsId = newsId,
                    Comments = comments
                });
            }
            catch (Exception e)
            {
                Log.Error(e, "List was not successful");
                throw;
            }            
        }

        public async Task<IActionResult> CreateCommentPartial(Guid newsId)
        {
            try
            {
                var comments = await _commentService.FindCommentsByNewsId(newsId);

                if (HttpContext.User.Identity.Name != null)
                {
                    var userEmail = HttpContext.User.Identity.Name;
                    var user = await _userService.GetUserByEmail(userEmail);
                    var userRoleName = _roleService.FindRoleById(user.RoleId).Result.Name;

                    return View(new CommentsListViewModel
                    {
                        NewsId = newsId,
                        Comments = comments
                    });
                }
                else
                {
                    return Ok();
                }
            }
            catch (Exception e)
            {
                Log.Error(e, "CreateCommentPartial was not successful");
                throw;
            }            
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody]CreateCommentViewModel comment)
        {
            try
            {
                var userEmail = HttpContext.User.Identity.Name;
                var user = await _userService.GetUserByEmail(userEmail);
                var userRoleName = _roleService.FindRoleById(user.RoleId).Result.Name;

                CommentDto commentDto = new CommentDto()
                {
                    FullName = user.FullName,
                    Id = new Guid(),
                    NewsId = comment.NewsId,
                    PublicationDate = DateTime.Now,
                    Text = comment.CommentText,
                    UserId = user.Id
                };

                await _commentService.AddComment(commentDto);

                return Ok();
            }
            catch (Exception e)
            {
                Log.Error(e, "Create was not successful");
                throw;
            }           
        }
    }
}

