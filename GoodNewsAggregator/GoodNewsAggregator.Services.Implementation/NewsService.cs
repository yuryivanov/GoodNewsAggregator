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
using System.Xml;
using System.ServiceModel.Syndication;
using AutoMapper;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;

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

        public async Task<NewsDto> GetNewsById(Guid? id)
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
                news = await _unitOfWork.News.FindBy(n => n != null)
                        .Select(n => _mapper.Map<NewsDto>(n)).ToListAsync();
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
                
        public Task<int> Aggregate()
        {
            throw new NotImplementedException();
        }

        //todo create the method for MVC:
        public async Task RateNews()
        {
            //process news without rates
            //todo process news for get text in next format:
            var newsText =
                "Апрельская зарплата, по данным Белстата, снова выросла. На этот раз +13,5 рублей за месяц. При этом зарплата минчан перевалила за 2 тысячи рублей в месяц, хотя в Могилеве и области насчитали в среднем всего 1118,4 рубля.Средняя начисленная зарплата в Беларуси в апреле составила 1398,2 рубля, сообщает Белстат. Это значит, что по сравнению с мартом она выросла на 13,5 рубля. После вычета налогов у среднестатистического белоруса на руках осталось 1202,45 рубля.Напомним, в марте этого года средняя заработная плата работников была 1384,7, в феврале — 1277,1 рубля.Топ зарплат в апреле выглядит так. Больше всех по традиции в апреле получили работники IT-сферы — 4684,2 рубля, за ними идут финансисты и страховщики (3505,2 рубля), работники грузового авиатранспорта (2956,3 рубля).Меньше всех получают работники сферы красоты и парикмахеры — 663,6 рубля до вычета налогов. За ними идут деятели сферы искусств и творческие работники (746,1 рубля). Библиотекари и музейные работники замыкают топ самых низких зарплат с показателем 751,9 рубля.При этом средняя зарплата минчан перевалила за 2 тысячи рублей в месяц и составила 2003,4 рубля. Неплохо. В минской области в апреле получали в среднем 1409,5 рублям, а в аутсайдерах, по традиции, жители Могилева и области (1118,4 рубля).";


            using (var httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders
                    .Accept
                    .Add(new MediaTypeWithQualityHeaderValue("application/json"));//ACCEPT header

                HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, "http://api.ispras.ru/texterra/v1/nlp?targetType=lemma&apikey=62e6440e1c9e5853620c0dd4ea3854b5e785b0eb")
                {
                    Content = new StringContent("[{\"text\":\"" + newsText + "\"}]",

                        Encoding.UTF8,
                        "application/json")
                };
                var response = await httpClient.SendAsync(request);

                var responseString = await response.Content.ReadAsStringAsync();
            }
        }
    }
}
