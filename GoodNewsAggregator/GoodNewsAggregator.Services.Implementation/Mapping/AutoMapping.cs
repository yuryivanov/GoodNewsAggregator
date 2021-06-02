using System;
using System.Collections.Generic;
using AutoMapper;
using GoodNewsAggregator.Core.DataTransferObjects;
using GoodNewsAggregator.DAL.Core.Entities;
using GoodNewsAggregator.DAL.Repositories.Implementation;

namespace NewsAggregators.Services.Implementation.Mapping
{
    public class AutoMapping : Profile
    {
        public AutoMapping()
        {
            CreateMap<News, NewsDto>().ForMember(dest
                => dest.RSS_Id, opt => opt.MapFrom(src => src.RSSId)
            );
            CreateMap<NewsDto, News>().ForMember(dest
                => dest.RSSId, opt => opt.MapFrom(src => src.RSS_Id)
            );
        }
    }
}