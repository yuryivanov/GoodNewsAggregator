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

            return await _dbContext.RSS
                .Select(sourse => _mapper.Map<RSSDto>(sourse))
                .ToListAsync(cancellationToken);
        }
    }
}
