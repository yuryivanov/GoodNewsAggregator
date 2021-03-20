using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GoodNewsAggregator.Core.DataTransferObjects;
using GoodNewsAggregator.Core.Services.Interfaces;
using GoodNewsAggregator.DAL.Core;

namespace GoodNewsAggregator.Services.Implementation
{
    public class NewsService : INewsService
    {
        private readonly GoodNewsAggregatorContext _context;
        private readonly INewsService _newsService;

        public NewsService(GoodNewsAggregatorContext context, INewsService newsService)
        {
            _newsService = newsService;
            _context = context;
        }

        public Task<NewsDto> AddNews(NewsDto news)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<NewsDto>> AddRange(IEnumerable<NewsDto> news)
        {
            throw new NotImplementedException();
        }

        public Task<NewsDto> EditNews(NewsDto news)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<NewsDto>> FindNews()
        {
            throw new NotImplementedException();
        }

        public Task<NewsDto> GetNewsById(Guid? id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<NewsDto>> GetNewsByRSSId(Guid? id)
        {
            throw new NotImplementedException();
        }

        public Task<NewsWithRSSAddressDto> GetNewsWithRSSAddressById(Guid? id)
        {
            throw new NotImplementedException();
        }

        public Task<NewsDto> RemoveNews(NewsDto news)
        {
            throw new NotImplementedException();
        }
    }
}
