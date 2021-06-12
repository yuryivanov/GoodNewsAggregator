using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GoodNewsAggregator.Core.DataTransferObjects;
using GoodNewsAggregator.Core.Services.Interfaces;
using GoodNewsAggregator.DAL.Core;
using GoodNewsAggregator.DAL.Core.Entities;
using GoodNewsAggregator.DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using GoodNewsAggregator.DAL.Repositories.Implementation;
using System.Xml;
using System.ServiceModel.Syndication;
using System.Text;
using System.Security.Cryptography;
using Serilog;
using AutoMapper;

namespace GoodNewsAggregator.Services.Implementation
{
    public class CommentService : ICommentService
    {
        private readonly IUnitOfWork _unitOfWork;
        public CommentService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<CommentDto>> FindCommentsByNewsId(Guid id)
        {
            var comments = await _unitOfWork.Comments.FindBy(comment => comment.NewsId.Equals(id)).OrderBy(comments=> comments.PublicationDate).ToListAsync();

            return comments.Select(comment => new CommentDto()
            {
                Id = comment.Id,
                NewsId = comment.NewsId,
                PublicationDate = comment.PublicationDate,
                Text = comment.Text,
                UserId = comment.UserId,
                FullName = (_unitOfWork.Users.FindBy(user => user.Id.Equals(comment.UserId)).FirstOrDefaultAsync()).Result.FullName
            }).ToList();
        }

        public async Task<CommentDto> FindCommentById(Guid id)
        {
            var comment = await _unitOfWork.Comments.FindBy(comment => comment.Id.Equals(id)).FirstOrDefaultAsync();

            return new CommentDto()
            {
                Id = comment.Id,
                NewsId = comment.NewsId,
                PublicationDate = comment.PublicationDate,
                Text = comment.Text,
                UserId = comment.UserId,
                FullName = (_unitOfWork.Users.FindBy(user => user.Id.Equals(comment.UserId)).FirstOrDefaultAsync()).Result.FullName
            };
        }

        public async Task<bool> AddComment(CommentDto comment)
        {
            try
            {
                if (comment != null)
                {
                    await _unitOfWork.Comments.Add(new Comment()
                    {
                        Id = comment.Id,
                        NewsId = comment.NewsId,
                        PublicationDate = comment.PublicationDate,
                        Text = comment.Text,
                        UserId = comment.UserId
                    });

                    await _unitOfWork.SaveChangesAsync();
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception e)
            {
                Log.Error(e, "AddComment process has an error");
                return false;
            }
        }
    }
}
