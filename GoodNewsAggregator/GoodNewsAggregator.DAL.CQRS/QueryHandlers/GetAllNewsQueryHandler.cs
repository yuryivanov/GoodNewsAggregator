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
    public class GetAllNewsQueryHandler : IRequestHandler<GetAllNewsQuery, IEnumerable<NewsDto>>
    {
        private readonly GoodNewsAggregatorContext _dbContext;
        private readonly IMapper _mapper;

        public GetAllNewsQueryHandler(GoodNewsAggregatorContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<IEnumerable<NewsDto>> Handle(GetAllNewsQuery request, CancellationToken cancellationToken)
        {

            return await _dbContext.News
                .Select(news => _mapper.Map<NewsDto>(news))
                .ToListAsync(cancellationToken);
        }
    }
}
