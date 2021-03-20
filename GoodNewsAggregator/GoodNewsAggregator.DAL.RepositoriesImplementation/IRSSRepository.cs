using GoodNewsAggregator.DAL.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoodNewsAggregator.DAL.Repositories.Interfaces
{
    public interface IRSSRepository
    {
        Task Add(RSS rss);
        Task<RSS> GetRSSById(Guid id);
        IQueryable<RSS> GetRSS();
        Task Update(RSS rss);
        Task Remove(Guid id);
    }
}
