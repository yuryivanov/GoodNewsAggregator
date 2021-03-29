using GoodNewsAggregator.DAL.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace GoodNewsAggregator.DAL.Repositories.Interfaces
{
    public interface IRepository<T> : IDisposable where T : class, IBaseEntity
    {
        Task<T> GetEntityById(Guid id);

        IQueryable<T> FindBy(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includes);

        Task Add(T entity);

        Task AddRange(IEnumerable<T> entity);       
        
        Task Update(T entity);

        Task Remove(T entity);

        Task RemoveRange(IEnumerable<T> entity);
    }
}
