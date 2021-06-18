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
using GoodNewsAggregator.DAL.CQRS.Commands;
using MediatR;
using GoodNewsAggregator.DAL.CQRS.Queries;
using System.Collections.Concurrent;
using Serilog;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.RegularExpressions;
using AutoMapper.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.IO;

namespace GoodNewsAggregator.Services.Implementation
{
    public class NewsCqsService : INewsService
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly OnlinerParser _onlinerParser;
        private readonly S13Parser _s13Parser;
        private readonly FourPdaParser _fourpdaParser;
        public NewsCqsService(IMediator mediator,
            IMapper mapper,
            IUnitOfWork unitOfWork,
            OnlinerParser onlinerParser,
            S13Parser s13Parser,
            FourPdaParser fourpdaParser)
        {
            _mediator = mediator;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _onlinerParser = onlinerParser;
            _s13Parser = s13Parser;
            _fourpdaParser = fourpdaParser;
        }

        public async Task<int> AddRangeNews(IEnumerable<NewsDto> news)
        {
            var command = new AddRangeNewsCommand() { News = news };
            int res = await _mediator.Send(command);
            return res;
        }

        public async Task<NewsDto> EditNews(NewsDto news)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<NewsDto>> FindNews()
        {
            //var news = await _unitOfWork.News.FindBy(n
            //            => n.Id.Equals(n.Id)).OrderByDescending(n => n.PublicationDate)
            //        .ToListAsync();

            //return news.Select(n => _mapper.Map<NewsDto>(n)).ToList();
            throw new NotImplementedException();
        }

        public async Task<NewsDto> GetNewsById(Guid? id)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<NewsDto>> GetNewsByRSSId(Guid? id)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<NewsWithRSSAddressDto>> GetNewsWithRSSAddressById(Guid? id)
        {
            var query = new GetNewsByIdWithRssAddressQuery() { Id = id };
            return await _mediator.Send(query);
        }

        public async Task<IEnumerable<NewsDto>> GetNewsInfoFromRssSourse(RSSDto rssSourse)
        {
            throw new NotImplementedException();
        }

        public async Task RemoveRangeNews(IEnumerable<NewsDto> news)
        {
            throw new NotImplementedException();
        }

        public async Task<int> Aggregate()
        {
            try
            {
                var query = new GetAllRssesQuery();
                var rsses = await _mediator.Send(query);

                //Collection for multiple threads work
                var news = new ConcurrentBag<NewsDto>();
                //Create News urls variable, so we don't repeat databaserequest each time we want get News urls
                var currentNewsUrls = await _mediator.Send(new GetAllExistingNewsUrlsQuery());

                Parallel.ForEach(rsses, rss =>
                {
                    //Create reader to read html from url
                    using (var reader = XmlReader.Create(rss.Address))
                    {
                        //Read html from url using previously created reader
                        var feed = SyndicationFeed.Load(reader);
                        reader.Close();
                        if (feed.Items.Any())
                        {
                            foreach (var syndicationItem in feed.Items
                            //Check that current item(news) hasn't been added to ConcurrentBag news yet
                            .Where(item => !currentNewsUrls.Any(url => url.Equals(item.Id))))
                            {
                                var newsDto = new NewsDto()
                                {
                                    Id = Guid.NewGuid(),
                                    RSS_Id = rss.Id,
                                    Address = syndicationItem.Id,
                                    Title = syndicationItem.Title.Text,
                                    Description = syndicationItem.Summary.Text.Replace("Читать далее…", ""), //clean from html(?)
                                    PublicationDate = syndicationItem.PublishDate.DateTime
                                };
                                news.Add(newsDto);
                            }
                        }
                    }
                });
                //Now ConcurrentBag news contains all news from rsses           


                Parallel.ForEach(news, singleNews =>
                {
                    if (singleNews.RSS_Id.Equals(new Guid("f68ffbd2-3ae5-4e80-be43-6021233c6ec9")))
                    {
                        var newsBody = _fourpdaParser.Parse(singleNews.Address);
                        singleNews.Text = newsBody.Result;
                    }
                    else if (singleNews.RSS_Id.Equals(new Guid("b637e8d9-dc89-4feb-951c-23d3baa7c48d")))
                    {
                        var newsBody = _s13Parser.Parse(singleNews.Address);
                        singleNews.Text = newsBody.Result;
                    }
                    else if (singleNews.RSS_Id.Equals(new Guid("972036b6-175f-4251-b2d9-296a77b65169")))
                    {
                        var newsBody = _onlinerParser.Parse(singleNews.Address);
                        singleNews.Text = newsBody.Result;
                    }
                });

                var command = new AddRangeNewsCommand() { News = news };
                int res = await _mediator.Send(command);
                //How many records have been added to the database? => res
                return res;
            }
            catch (Exception e)
            {
                Log.Error(e, $"{e.Message}");
            }
            //If the method fails
            return 0;
        }

        public async Task RateNews()
        {
            //Get dictionary<word, mark>
            dynamic jsonFile = JsonConvert.DeserializeObject(File.ReadAllText("D:\\it-academy\\New folder (2)\\GoodNewsAggregator\\GoodNewsAggregator\\GoodNewsAggregator.WebAPI\\appsettings.json"));
            var url = $"{jsonFile["CoefficientStrings"]["Coefficients"]}";

            byte[] bytes = Encoding.Default.GetBytes(File.ReadAllText($"{url}"));
            string myString = Encoding.UTF8.GetString(bytes);

            var coefficients = JsonConvert.DeserializeObject<Coefficients>(myString);

            //Get News
            var allNews = await GetNewsWithRSSAddressById(null);
            var allNewsWithoutGoodnessCoefficient = allNews.Where(x => x.GoodnessCoefficient == null).ToList();

            var iteration = 0;
            //var singleNews = allNewsWithoutGoodnessCoefficient[0];            
            foreach (var singleNews in allNewsWithoutGoodnessCoefficient)
            {
                iteration += 1;
                var newsText = singleNews.Text;

                //Get news text without html tags
                string StripHTML(string input)
                {
                    return Regex.Replace(input, "<.*?>", String.Empty);
                }
                                
                int start = newsText.IndexOf("<script type=\"text/javascript\">");
                int end = newsText.IndexOf("</script>");
                if (start != -1 && end != -1)
                {
                    newsText = newsText.Substring(0, start) + "" +
                      newsText.Substring(end + 9);
                }                

                newsText = StripHTML(newsText);
                newsText = newsText.Replace("\t", "").Replace("\n", "").Replace("\r", "");

                //Send POST request to do lemmatization
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

                    var responseString = response.Content.ReadAsStringAsync();

                    //method for parsing newsText to words
                    List<string> ExtractFromBody(string body, string start, string end)
                    {
                        List<string> matched = new List<string>();

                        int indexStart = 0;
                        int indexEnd = 0;

                        bool exit = false;
                        while (!exit)
                        {
                            indexStart = body.IndexOf(start);

                            if (indexStart != -1)
                            {
                                indexEnd = indexStart + body.Substring(indexStart).IndexOf(end);

                                matched.Add(body.Substring(indexStart + start.Length, indexEnd - indexStart - start.Length));

                                body = body.Substring(indexEnd + end.Length);
                            }
                            else
                            {
                                exit = true;
                            }
                        }

                        return matched;
                    }

                    //Get List of words from newsText
                    List<string> wordsFromTheResponseString = ExtractFromBody(responseString.Result, "\"value\":\"", "\"}");

                    //Update News with a new GoodnessCoefficient
                    double sum = 0;
                    double number = 0;
                    foreach (var item in wordsFromTheResponseString)
                    {
                        if (coefficients.CoefficientsForEachWord.ContainsKey(item))
                        {
                            sum += coefficients.CoefficientsForEachWord[item];
                            number += 1;
                        }
                    }
                    if (number != 0)
                    {
                        singleNews.GoodnessCoefficient = Math.Round(sum/number);
                    }
                    else
                    {
                        singleNews.GoodnessCoefficient = 0;
                    }                   

                    int res = await _mediator.Send(new EditNewsCommand()
                    {
                        News = new NewsDto
                        {
                            Id = singleNews.Id,
                            Address = singleNews.Address,
                            Description = singleNews.Description,
                            GoodnessCoefficient = singleNews.GoodnessCoefficient,
                            PublicationDate = singleNews.PublicationDate,
                            RSS_Id = singleNews.RSS_Id,
                            Text = singleNews.Text,
                            Title = singleNews.Title
                        }
                    });
                }
                if (iteration%15==0)
                {
                    await Task.Delay(61000);
                }                
            }
        }
    }

    public class Coefficients
    {
        public Dictionary<string, int> CoefficientsForEachWord { get; set; }
    }
}
