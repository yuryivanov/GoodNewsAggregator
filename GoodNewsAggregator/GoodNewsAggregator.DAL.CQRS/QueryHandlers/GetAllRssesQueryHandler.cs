using System;
using GoodNewsAggregator.DAL.CQRS.Queries;
using GoodNewsAggregator.DAL.Core;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using GoodNewsAggregator.Core.DataTransferObjects;
using AutoMapper;
using MediatR;
using System.Threading;
using System.Linq;
using Serilog;

namespace GoodNewsAggregator.DAL.CQRS.QueryHandlers
{
    public class GetAllRssesQueryHandler : IRequestHandler<GetAllRssesQuery, IEnumerable<RSSDto>>
    {
        private readonly GoodNewsAggregatorContext _dbContext;
        private readonly IMapper _mapper;

        public GetAllRssesQueryHandler(GoodNewsAggregatorContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<IEnumerable<RSSDto>> Handle(GetAllRssesQuery request, CancellationToken cancellationToken)
        {
            try
            {
                return await _dbContext.RSS
                .Select(sourse => _mapper.Map<RSSDto>(sourse))
                .ToListAsync(cancellationToken);
            }
            catch (Exception e)
            {
                Log.Error(e, "GetAllRssesQueryHandler was not successful");
                throw;
            }            
        }
    }
}
