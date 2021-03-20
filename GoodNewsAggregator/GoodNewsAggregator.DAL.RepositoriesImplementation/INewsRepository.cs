using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GoodNewsAggregator.DAL.Core.Entities;

namespace GoodNewsAggregator.DAL.Repositories.Interfaces
{
    public interface INewsRepository
    {
        Task Add(News news);
        Task<News> GetNewsById(Guid id);
        IQueryable<News> GetNews();
        Task Update(News news);
        Task Remove(Guid id);
    }
}
