﻿using AutoMapper;
using GoodNewsAggregator.Core.DataTransferObjects;
using GoodNewsAggregator.DAL.Core.Entities;

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

            CreateMap<RSS, RSSDto>();
            CreateMap<RSSDto, RSS>();

            CreateMap<Comment, CommentDto>();
            CreateMap<CommentDto, Comment>();

            CreateMap<User, UserDto>();
            CreateMap<UserDto, User>();

            CreateMap<RefreshToken, RefreshTokenDto>();
            CreateMap<RefreshTokenDto, RefreshToken>();
        }
    }
}