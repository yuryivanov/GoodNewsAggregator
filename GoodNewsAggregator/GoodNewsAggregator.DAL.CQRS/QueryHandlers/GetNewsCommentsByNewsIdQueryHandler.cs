using System;
using GoodNewsAggregator.DAL.CQRS.Queries;
using GoodNewsAggregator.DAL.Core;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using GoodNewsAggregator.DAL.Core.Entities;
using GoodNewsAggregator.Core.DataTransferObjects;
using AutoMapper;
using MediatR;
using System.Threading;
using System.Linq;

namespace GoodNewsAggregator.DAL.CQRS.QueryHandlers
{
    public class GetNewsCommentsByNewsIdQueryHandler : IRequestHandler<GetNewsCommentsByNewsIdQuery, IEnumerable<CommentDto>>
    {
        private readonly GoodNewsAggregatorContext _dbContext;
        private readonly IMapper _mapper;

        public GetNewsCommentsByNewsIdQueryHandler(GoodNewsAggregatorContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<IEnumerable<CommentDto>> Handle(GetNewsCommentsByNewsIdQuery request, CancellationToken cancellationToken)
        {
            News newsWithComments = await _dbContext.News.Include(news => news.CommentCollection)
                .FirstOrDefaultAsync(news => news.Id.Equals(request.Id), cancellationToken);
            if (newsWithComments != null && newsWithComments.CommentCollection != null)
            {
                return newsWithComments.CommentCollection.Select(x => _mapper.Map<CommentDto>(x)).ToList();
            }
            else
            {
                return null;
            }
        }

    }
}

