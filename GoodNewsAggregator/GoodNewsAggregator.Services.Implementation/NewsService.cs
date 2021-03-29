using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GoodNewsAggregator.Core.DataTransferObjects;
using GoodNewsAggregator.Core.Services.Interfaces;
using GoodNewsAggregator.DAL.Core;
using GoodNewsAggregator.DAL.Core.Entities;
using GoodNewsAggregator.DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using GoodNewsAggregator.DAL.Repositories.Implementation;

namespace GoodNewsAggregator.Services.Implementation
{
    public class NewsService : INewsService
    {
        private readonly IUnitOfWork _unitOfWork;
        public NewsService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task AddRangeNews(IEnumerable<NewsDto> news)
        {
            var addedNews = news.Select(entity => new News()
            {
                Id = entity.Id,
                Address = entity.Address,
                Description = entity.Description,
                GoodnessCoefficient = entity.GoodnessCoefficient,
                PublicationDate = entity.PublicationDate,
                RSSId = entity.RSS_Id,
                Text = entity.Text,
                Title = entity.Title
            }).ToList();

            await _unitOfWork.News.AddRange(addedNews);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<NewsDto> EditNews(NewsDto news)
        {
            var oldNews = await _unitOfWork.News.GetEntityById(news.Id);

            var EditedNews = new News()
            {
                Id = news.Id,
                Address = news.Address,
                Description = news.Description,
                GoodnessCoefficient = news.GoodnessCoefficient,
                PublicationDate = news.PublicationDate,
                RSSId = news.RSS_Id,
                Text = news.Text,
                Title = news.Title
            };

            await _unitOfWork.News.Remove(oldNews);
            await _unitOfWork.News.Add(EditedNews);
            await _unitOfWork.SaveChangesAsync();

            return await GetNewsById(EditedNews.Id);
        }

        public async Task<IEnumerable<NewsDto>> FindNews()
        {            
            var news = await _unitOfWork.News.FindBy(n
                        => n.Id.Equals(n.Id))
                    .ToListAsync();


            return news.Select(n => new NewsDto()
            {
                Id = n.Id,
                Address = n.Address,
                Description = n.Description,
                GoodnessCoefficient = n.GoodnessCoefficient,
                PublicationDate = n.PublicationDate,
                RSS_Id = n.RSSId,
                Text = n.Text,
                Title = n.Title
            }).ToList();
        }

        public async Task<NewsDto> GetNewsById(Guid? id)
        {
            var entity = await _unitOfWork.News.GetEntityById(id.GetValueOrDefault());
            return new NewsDto()
            {
                Id = entity.Id,
                Address = entity.Address,
                Description = entity.Description,
                GoodnessCoefficient = entity.GoodnessCoefficient,
                PublicationDate = entity.PublicationDate,
                RSS_Id = entity.RSSId,
                Text = entity.Text,
                Title = entity.Title
            };
        }

        public async Task<IEnumerable<NewsDto>> GetNewsByRSSId(Guid? id)
        {
            IEnumerable<NewsDto> news;
            if (id.HasValue)
            {
                //Chosen news
                news = await _unitOfWork.News.FindBy(n => n.Id.Equals(n.Id), n => n).Where(news => news.RSSId.Equals(id.GetValueOrDefault())).Select(entity => new NewsDto()
                {
                    Id = entity.Id,
                    Address = entity.Address,
                    Description = entity.Description,
                    GoodnessCoefficient = entity.GoodnessCoefficient,
                    PublicationDate = entity.PublicationDate,
                    RSS_Id = entity.RSSId,
                    Text = entity.Text,
                    Title = entity.Title
                }).ToListAsync();
            }
            else
            {
                //All news
                news = await _unitOfWork.News.FindBy(n => n.Id.Equals(n.Id), n => n).Select(entity => new NewsDto()
                {
                    Id = entity.Id,
                    Address = entity.Address,
                    Description = entity.Description,
                    GoodnessCoefficient = entity.GoodnessCoefficient,
                    PublicationDate = entity.PublicationDate,
                    RSS_Id = entity.RSSId,
                    Text = entity.Text,
                    Title = entity.Title
                }).ToListAsync();
            }

            return news;
        }

        public async Task<IEnumerable<NewsWithRSSAddressDto>> GetNewsWithRSSAddressById(Guid? id)
        {
            IEnumerable<NewsWithRSSAddressDto> newsWithRSSAddress;
            if (id.HasValue)
            {
                //Chosen news
                newsWithRSSAddress = await _unitOfWork.News.FindBy(n => n.Id.Equals(n.Id), n => n).Where(news => news.Id.Equals(id.GetValueOrDefault())).Select(n => new NewsWithRSSAddressDto()
                {
                    Id = n.Id,
                    Address = n.Address,
                    RSSAddress = n.RSS.Address,
                    Description = n.Description,
                    GoodnessCoefficient = n.GoodnessCoefficient,
                    PublicationDate = n.PublicationDate,
                    RSS_Id = n.RSSId,
                    Text = n.Text,
                    Title = n.Title
                }).ToListAsync();
            }
            else
            {
                //All news
                newsWithRSSAddress = await _unitOfWork.News.FindBy(n => n.Id.Equals(n.Id), n => n.RSS).Select(n => new NewsWithRSSAddressDto()
                {
                    Id = n.Id,
                    Address = n.Address,
                    RSSAddress = n.RSS.Address,
                    Description = n.Description,
                    GoodnessCoefficient = n.GoodnessCoefficient,
                    PublicationDate = n.PublicationDate,
                    RSS_Id = n.RSSId,
                    Text = n.Text,
                    Title = n.Title
                }).ToListAsync();
            }

            return newsWithRSSAddress;
        }

        public async Task RemoveRangeNews(IEnumerable<NewsDto> news)
        {
            var removedNews = news.Select(entity => new News()
            {
                Id = entity.Id,
                Address = entity.Address,
                Description = entity.Description,
                GoodnessCoefficient = entity.GoodnessCoefficient,
                PublicationDate = entity.PublicationDate,
                RSSId = entity.RSS_Id,
                Text = entity.Text,
                Title = entity.Title
            }).ToList();

            await _unitOfWork.News.RemoveRange(removedNews);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}
