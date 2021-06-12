using GoodNewsAggregator.Core.DataTransferObjects;
using GoodNewsAggregator.DAL.CQRS.Queries;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GoodNewsAggregator.DAL.CQRS.QueryHandlers
{
    public interface IQueryHandler<T> where T : IQuery
    {
        Task<IDtoModel> Handle(T query);
    }
}
