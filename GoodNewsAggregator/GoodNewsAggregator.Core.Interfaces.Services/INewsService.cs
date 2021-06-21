using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GoodNewsAggregator.Core.DataTransferObjects;

namespace GoodNewsAggregator.Core.Services.Interfaces
{
    public interface INewsService
    {
        Task<IEnumerable<NewsDto>> GetNewsByRSSId(Guid? id); 
        Task<NewsDto> GetNewsById(Guid? id); 
        Task<IEnumerable<NewsWithRSSAddressDto>> GetNewsWithRSSAddressById(Guid? id); 
        Task<NewsDto> EditNews(NewsDto news);
        Task RemoveRangeNews(IEnumerable<NewsDto> news);
        Task<IEnumerable<NewsDto>> FindNews(); 
        Task<int> AddRangeNews(IEnumerable<NewsDto> news); 
        Task<IEnumerable<NewsDto>> GetNewsInfoFromRssSourse(RSSDto rssSourse);

        Task<int> Aggregate();
        Task RateNews();
    }
}
