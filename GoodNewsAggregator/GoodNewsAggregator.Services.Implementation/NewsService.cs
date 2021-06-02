﻿using System;
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
using System.Xml;
using System.ServiceModel.Syndication;
using AutoMapper;

namespace GoodNewsAggregator.Services.Implementation
{
    public class NewsService : INewsService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public NewsService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
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
                        => n.Id.Equals(n.Id)).OrderByDescending(n=>n.PublicationDate)
                    .ToListAsync();


            //return news.Select(n => new NewsDto()
            //{
            //    Id = n.Id,
            //    Address = n.Address,
            //    Description = n.Description,
            //    GoodnessCoefficient = n.GoodnessCoefficient,
            //    PublicationDate = n.PublicationDate,
            //    RSS_Id = n.RSSId,
            //    Text = n.Text,
            //    Title = n.Title
            //}).ToList();

            return news.Select(n => _mapper.Map<NewsDto>(n)).ToList();
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

        public async Task<IEnumerable<NewsDto>> GetNewsInfoFromRssSourse(RSSDto rssSourse)
        {
            var news = new List<NewsDto>();
            using (var reader = XmlReader.Create(rssSourse.Address))
            {
                var feed = SyndicationFeed.Load(reader);
                reader.Close();
                if (feed.Items.Any())
                {
                    var currentNewsUrls = await _unitOfWork.News
                        .FindBy(n
                        => n.Id.Equals(n.Id))//rssSourseId must be not nullable
                        .Select(n => n.Address)
                        .ToListAsync();

                    foreach (var syndicationItem in feed.Items)
                    {
                        //var syndicationItems = feed.Items;
                        //Parallel.ForEach(syndicationItems, syndicationItem =>
                        //{
                        if (!currentNewsUrls.Any(url => url.Equals(syndicationItem.Id)))
                        {
                            var newsDto = new NewsDto()
                            {
                                Id = Guid.NewGuid(),
                                RSS_Id = rssSourse.Id,
                                Address = syndicationItem.Id,
                                Title = syndicationItem.Title.Text,
                                Description = syndicationItem.Summary.Text.Replace("Читать далее…", ""), //clean from html(?)
                                PublicationDate = syndicationItem.PublishDate.DateTime
                            };
                            news.Add(newsDto);
                        }
                        //});
                    }
                }
            }
            return news;
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
