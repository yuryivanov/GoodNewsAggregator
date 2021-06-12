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

namespace GoodNewsAggregator.Services.Implementation
{
    public class RSSService : IRSSService
    {
        private readonly IUnitOfWork _unitOfWork;
        public RSSService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<RSSDto>> FindRss()
        {
            var rSSes = await _unitOfWork.RSS.FindBy(n
                        => n.Id.Equals(n.Id))
                    .ToListAsync();


            return rSSes.Select(n => new RSSDto()
            {
                Id = n.Id,
                Address = n.Address
            }).ToList();
        }

        public async Task<RSSDto> FindRssById(Guid id)
        {
            var rSS = await _unitOfWork.RSS.GetEntityById(id);

            if (rSS != null)
            {
                return new RSSDto()
                {
                    Id = rSS.Id,
                    Address = rSS.Address
                };
            }
            else
            {
                return null;
            }            
        }
    }
}
