using AutoMapper;
using GoodNewsAggregator.DAL.Core;
using GoodNewsAggregator.DAL.Core.Entities;
using GoodNewsAggregator.DAL.CQRS.Commands;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace GoodNewsAggregator.DAL.CQRS.CommandHandlers
{
    public class AddRangeNewsCommandHandler : IRequestHandler<AddRangeNewsCommand, int>
    {
        private readonly GoodNewsAggregatorContext _dbContext;
        private readonly IMapper _mapper;

        public AddRangeNewsCommandHandler(GoodNewsAggregatorContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<int> Handle(AddRangeNewsCommand request, CancellationToken cancellationToken)
        {
            var addedNews = request.News.Select(n => _mapper.Map<News>(n)).ToList();

            await _dbContext.News.AddRangeAsync(addedNews, cancellationToken);
            //How many records in db are changed, if 0 - request failed
            return await _dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}
