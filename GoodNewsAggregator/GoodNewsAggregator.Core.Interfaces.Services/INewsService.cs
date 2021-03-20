﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using GoodNewsAggregator.Core.DataTransferObjects;

namespace GoodNewsAggregator.Core.Services.Interfaces
{
    public interface INewsService
    {
        Task<IEnumerable<NewsDto>> GetNewsByRSSId(Guid? id);
        Task<NewsDto> GetNewsById(Guid? id);
        Task<NewsDto> AddNews(NewsDto news);
        Task<NewsWithRSSAddressDto> GetNewsWithRSSAddressById(Guid? id);
        Task<NewsDto> EditNews(NewsDto news);
        Task<NewsDto> RemoveNews(NewsDto news);
        Task<IEnumerable<NewsDto>> FindNews();
        Task<IEnumerable<NewsDto>> AddRange(IEnumerable<NewsDto> news);
    }
}