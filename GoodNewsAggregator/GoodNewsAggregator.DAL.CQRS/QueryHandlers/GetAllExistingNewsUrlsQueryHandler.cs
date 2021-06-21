using System;
using GoodNewsAggregator.DAL.CQRS.Queries;
using GoodNewsAggregator.DAL.Core;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using MediatR;
using System.Threading;
using System.Linq;
using Serilog;

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
            try
            {
                return await _dbContext.News
                .Select(news => news.Address)
                .ToListAsync(cancellationToken);
            }
            catch (Exception e)
            {
                Log.Error(e, "GetAllExistingNewsUrlsQueryHandler was not successful");
                throw;
            }            
        }
    }
}
