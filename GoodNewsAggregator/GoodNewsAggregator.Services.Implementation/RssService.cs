using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GoodNewsAggregator.Core.DataTransferObjects;
using GoodNewsAggregator.Core.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using GoodNewsAggregator.DAL.Repositories.Implementation;
using Serilog;

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
            try
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
            catch (Exception e)
            {
                Log.Error(e, "FindRssById was not successful");
                throw;
            }                     
        }
    }
}
