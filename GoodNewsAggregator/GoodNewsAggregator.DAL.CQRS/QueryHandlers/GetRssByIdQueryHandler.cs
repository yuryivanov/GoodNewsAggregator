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

namespace GoodNewsAggregator.DAL.CQRS.QueryHandlers
{
    public class GetRssByIdQueryHandler : IQueryHandler<GetRssByIdQuery>
    {
        private readonly GoodNewsAggregatorContext _dbContext;
        private readonly IMapper _mapper; 

        public GetRssByIdQueryHandler(GoodNewsAggregatorContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<IDtoModel> Handle(GetRssByIdQuery query)
        {
            return _mapper.Map<RSSDto>(await _dbContext.RSS.FirstOrDefaultAsync(rss => rss.Id.Equals(query.Id)));
        }
    }
}
