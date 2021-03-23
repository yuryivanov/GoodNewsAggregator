using GoodNewsAggregator.DAL.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GoodNewsAggregator.DAL.Repositories.Implementation
{
    public interface IUnitOfWork
    {
        INewsRepository News { get; }
        IRSSRepository RSS { get; }
        Task<int> SaveChangesAsync();
    }
}
