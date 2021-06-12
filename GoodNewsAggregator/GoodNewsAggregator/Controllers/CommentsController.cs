using GoodNewsAggregator.Core.DataTransferObjects;
using GoodNewsAggregator.Core.Services.Interfaces;
using GoodNewsAggregator.DAL.Repositories.Implementation;
using GoodNewsAggregator.Models.ViewModels.Account;
using GoodNewsAggregator.Models.ViewModels.Comment;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
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
            var comments = await _commentService.FindCommentsByNewsId(newsId);

            return View(new CommentsListViewModel
            {
            NewsId = newsId,
            Comments = comments
            });
        }

        public async Task<IActionResult> CreateCommentPartial(Guid newsId)
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

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody]CreateCommentViewModel comment)
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
    }
}

