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
    public class GetNewsByIdWithRssAddressQueryHandler : IRequestHandler<GetNewsByIdWithRssAddressQuery, IEnumerable<NewsWithRSSAddressDto>>
    {
        private readonly GoodNewsAggregatorContext _dbContext;
        private readonly IMapper _mapper;

        public GetNewsByIdWithRssAddressQueryHandler(GoodNewsAggregatorContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<IEnumerable<NewsWithRSSAddressDto>> Handle(GetNewsByIdWithRssAddressQuery request, CancellationToken cancellationToken)
        {
            News newsWithRSSAddress = await _dbContext.News.Include(news => news.RSS)
                .FirstOrDefaultAsync(news => news.Id.Equals(request.Id), cancellationToken);
            if (newsWithRSSAddress != null)
            {
                return new List<NewsWithRSSAddressDto>()
                {
                    new NewsWithRSSAddressDto()
                {
                    Id = newsWithRSSAddress.Id,
                    Address = newsWithRSSAddress.Address,
                    RSSAddress = newsWithRSSAddress.RSS.Address,
                    Description = newsWithRSSAddress.Description,
                    GoodnessCoefficient = newsWithRSSAddress.GoodnessCoefficient,
                    PublicationDate = newsWithRSSAddress.PublicationDate,
                    RSS_Id = newsWithRSSAddress.RSSId,
                    Text = newsWithRSSAddress.Text,
                    Title = newsWithRSSAddress.Title
                }
                };
            }
            else
            {
                var news = await _dbContext.News.Include(news => news.RSS).Where(x => x != null).ToListAsync();

                return news.Select(newsWithRSSAddress => new NewsWithRSSAddressDto()
                {
                    Id = newsWithRSSAddress.Id,
                    Address = newsWithRSSAddress.Address,
                    RSSAddress = newsWithRSSAddress.RSS.Address,
                    Description = newsWithRSSAddress.Description,
                    GoodnessCoefficient = newsWithRSSAddress.GoodnessCoefficient,
                    PublicationDate = newsWithRSSAddress.PublicationDate,
                    RSS_Id = newsWithRSSAddress.RSSId,
                    Text = newsWithRSSAddress.Text,
                    Title = newsWithRSSAddress.Title
                }).ToList();
            }
        }

    }
}

