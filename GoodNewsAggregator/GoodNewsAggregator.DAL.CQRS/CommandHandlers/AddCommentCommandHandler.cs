using AutoMapper;
using GoodNewsAggregator.DAL.Core;
using GoodNewsAggregator.DAL.Core.Entities;
using GoodNewsAggregator.DAL.CQRS.Commands;
using MediatR;
using Serilog;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace GoodNewsAggregator.DAL.CQRS.CommandHandlers
{
    public class AddCommentCommandHandler : IRequestHandler<AddCommentCommand, int>
    {
        private readonly GoodNewsAggregatorContext _dbContext;
        private readonly IMapper _mapper;

        public AddCommentCommandHandler(GoodNewsAggregatorContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<int> Handle(AddCommentCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var comment = _mapper.Map<Comment>(request.Comment);

                await _dbContext.Comments.AddAsync(comment, cancellationToken);

                //How many records in db are changed, if 0 - request failed
                return await _dbContext.SaveChangesAsync(cancellationToken);
            }
            catch (Exception e)
            {
                Log.Error(e, "AddCommentCommandHandler was not successful");
                throw;
            }
        }
    }
}
