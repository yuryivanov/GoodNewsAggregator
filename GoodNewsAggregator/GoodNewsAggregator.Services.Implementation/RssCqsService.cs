using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GoodNewsAggregator.Core.DataTransferObjects;
using GoodNewsAggregator.Core.Services.Interfaces;
using MediatR;
using GoodNewsAggregator.DAL.CQRS.Queries;
using Serilog;

namespace GoodNewsAggregator.Services.Implementation
{
    public class RssCqsService : IRSSService
    {
        private readonly IMediator _mediator;
        public RssCqsService(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<IEnumerable<RSSDto>> FindRss()
        {
            try
            {
                //MediatR - dispatcher that send requests to db
                var query = new GetAllRssesQuery();
                return await _mediator.Send(query);
            }
            catch (Exception e)
            {
                Log.Error(e, "FindRss was not successful");
                throw;
            }           
        }

        public async Task<RSSDto> FindRssById(Guid id)
        {
            try
            {
                var query = new GetRssByIdQuery() { Id = id };
                return await _mediator.Send(query);
            }
            catch (Exception e)
            {
                Log.Error(e, "FindRssById was not successful");
                throw;
            }           
        }
    }
}
