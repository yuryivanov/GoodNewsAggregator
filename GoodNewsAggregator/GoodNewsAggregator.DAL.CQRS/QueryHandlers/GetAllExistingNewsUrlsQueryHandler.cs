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
    public class GetAllExistingNewsUrlsQueryHandler : IRequestHandler<GetAllExistingNewsUrlsQuery, List<string>>
    {
        private readonly GoodNewsAggregatorContext _dbContext;

        public GetAllExistingNewsUrlsQueryHandler(GoodNewsAggregatorContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<string>> Handle(GetAllExistingNewsUrlsQuery request, CancellationToken cancellationToken)
        {

            return await _dbContext.News
                .Select(news => news.Address)
                .ToListAsync(cancellationToken);
        }
    }
}
