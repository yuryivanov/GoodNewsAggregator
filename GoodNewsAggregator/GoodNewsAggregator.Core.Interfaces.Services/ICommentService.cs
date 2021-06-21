using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GoodNewsAggregator.Core.DataTransferObjects;

namespace GoodNewsAggregator.Core.Services.Interfaces
{
    public interface ICommentService
    {
        Task<IEnumerable<CommentDto>> FindCommentsByNewsId(Guid id);
        Task<CommentDto> FindCommentById(Guid id);
        Task<bool> AddComment(CommentDto comment);
    }
}
