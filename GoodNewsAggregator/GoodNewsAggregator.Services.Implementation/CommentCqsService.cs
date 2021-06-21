using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GoodNewsAggregator.Core.DataTransferObjects;
using GoodNewsAggregator.Core.Services.Interfaces;
using Serilog;
using MediatR;
using GoodNewsAggregator.DAL.CQRS.Queries;
using GoodNewsAggregator.DAL.CQRS.Commands;

namespace GoodNewsAggregator.Services.Implementation
{
    public class CommentCqsService : ICommentService
    {
        private readonly IMediator _mediator;
        public CommentCqsService(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<IEnumerable<CommentDto>> FindCommentsByNewsId(Guid id)
        {
            try
            {
                var query = new GetNewsCommentsByNewsIdQuery() { Id = id };
                return await _mediator.Send(query);
            }
            catch (Exception e)
            {
                Log.Error(e, "FindCommentsByNewsId was not successful");
                throw;
            }            
        }             

        public async Task<bool> AddComment(CommentDto comment)
        {
            try
            {
                var command = new AddCommentCommand() { Comment = comment };
                int res = await _mediator.Send(command);
                if (res > 0)
                {
                    return true;
                }
                return false;
            }
            catch (Exception e)
            {
                Log.Error(e, "AddComment was not successful");
                throw;
            }            
        }

        public Task<CommentDto> FindCommentById(Guid id)
        {
            throw new NotImplementedException();
        }
    }
}
