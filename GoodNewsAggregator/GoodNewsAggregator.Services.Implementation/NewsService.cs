using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GoodNewsAggregator.Core.DataTransferObjects;
using GoodNewsAggregator.Core.Services.Interfaces;
using GoodNewsAggregator.DAL.Core.Entities;
using Microsoft.EntityFrameworkCore;
using GoodNewsAggregator.DAL.Repositories.Implementation;
using System.Xml;
using System.ServiceModel.Syndication;
using AutoMapper;
using Serilog;

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

        public async Task<int> AddRangeNews(IEnumerable<NewsDto> news)
        {
            try
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
                return 1;
            }
            catch (Exception e)
            {
                Log.Error(e, "AddRangeNews was not successful");
                throw;
            }            
        }

        public async Task<NewsDto> EditNews(NewsDto news)
        {
            try
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
            catch (Exception e)
            {
                Log.Error(e, "EditNews was not successful");
                throw;
            }            
        }

        public async Task<IEnumerable<NewsDto>> FindNews()
        {
            try
            {
                var news = await _unitOfWork.News.FindBy(n
                        => n.Id.Equals(n.Id)).OrderByDescending(n => n.PublicationDate)
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
            catch (Exception e)
            {
                Log.Error(e, "FindNews was not successful");
                throw;
            }           
        }

        public async Task<NewsDto> GetNewsById(Guid? id)
        {
            try
            {
                var entity = await _unitOfWork.News.GetEntityById(id.GetValueOrDefault());

                if (entity != null)
                {
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
                else
                {
                    return null;
                }
            }
            catch (Exception e)
            {
                Log.Error(e, "GetNewsById was not successful");
                throw;
            }           
        }

        public async Task<IEnumerable<NewsDto>> GetNewsByRSSId(Guid? id)
        {
            try
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
                    news = await _unitOfWork.News.FindBy(n => n != null)
                            .Select(n => _mapper.Map<NewsDto>(n)).ToListAsync();
                }

                return news;
            }
            catch (Exception e)
            {
                Log.Error(e, "GetNewsByRSSId was not successful");
                throw;
            }           
        }

        public async Task<IEnumerable<NewsWithRSSAddressDto>> GetNewsWithRSSAddressById(Guid? id)
        {
            try
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
            catch (Exception e)
            {
                Log.Error(e, "GetNewsWithRSSAddressById was not successful");
                throw;
            }            
        }

        public async Task<IEnumerable<NewsDto>> GetNewsInfoFromRssSourse(RSSDto rssSourse)
        {
            try
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
            catch (Exception e)
            {
                Log.Error(e, "GetNewsInfoFromRssSourse was not successful");
                throw;
            }            
        }

        public async Task RemoveRangeNews(IEnumerable<NewsDto> news)
        {
            try
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
            catch (Exception e)
            {
                Log.Error(e, "RemoveRangeNews was not successful");
                throw;
            }            
        }
                
        public Task<int> Aggregate()
        {
            throw new NotImplementedException();
        }

        public async Task RateNews()
        {
            throw new NotImplementedException();            
        }
    }
}
