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
using MediatR;
using GoodNewsAggregator.DAL.CQRS.Queries;

namespace GoodNewsAggregator.Services.Implementation
{
    public class RssCqsService : IRSSService
    {
        private readonly IMapper _mapper;
        private readonly IMediator _mediator;
        public RssCqsService(IMapper mapper, IMediator mediator)
        {
            _mapper = mapper;
            _mediator = mediator;
        }

        public async Task<IEnumerable<RSSDto>> FindRss()
        {
            //MediatR - dispatcher that send requests to db
            var query = new GetAllRssesQuery();
            return await _mediator.Send(query);
        }

        public async Task<RSSDto> FindRssById(Guid id)
        {
            var query = new GetRssByIdQuery() { Id = id};
            return await _mediator.Send(query);
        }
    }
}
