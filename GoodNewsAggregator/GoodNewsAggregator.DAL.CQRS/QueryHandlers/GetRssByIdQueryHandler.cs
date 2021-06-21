using System;
using GoodNewsAggregator.DAL.CQRS.Queries;
using GoodNewsAggregator.DAL.Core;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using GoodNewsAggregator.Core.DataTransferObjects;
using AutoMapper;
using MediatR;
using System.Threading;
using Serilog;

namespace GoodNewsAggregator.DAL.CQRS.QueryHandlers
{
    public class GetRssByIdQueryHandler : IRequestHandler<GetRssByIdQuery, RSSDto>
    {
        private readonly GoodNewsAggregatorContext _dbContext;
        private readonly IMapper _mapper; 

        public GetRssByIdQueryHandler(GoodNewsAggregatorContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<RSSDto> Handle(GetRssByIdQuery request, CancellationToken cancellationToken)
        {
            try
            {
                return _mapper.Map<RSSDto>(await _dbContext.RSS.FirstOrDefaultAsync(rss => rss.Id.Equals(request.Id), cancellationToken));
            }
            catch (Exception e)
            {
                Log.Error(e, "GetRssByIdQueryHandler was not successful");
                throw;
            }            
        }
    }
}
