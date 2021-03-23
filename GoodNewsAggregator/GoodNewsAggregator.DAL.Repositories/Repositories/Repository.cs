using GoodNewsAggregator.DAL.Core;
using GoodNewsAggregator.DAL.Core.Entities;
using GoodNewsAggregator.DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace GoodNewsAggregator.DAL.Repositories.Implementation
{
    public abstract class Repository<T> : IRepository<T> where T : class, IBaseEntity
    {
        protected readonly GoodNewsAggregatorContext Db;
        protected readonly DbSet<T> Table;

        protected Repository(GoodNewsAggregatorContext context, DbSet<T> table)
        {
            Db = context;
            Table = Db.Set<T>(); //return table with type T
        }
        public async Task<T> GetEntityById(Guid id)
        {
            return await Table.FirstOrDefaultAsync(entity => entity.Id.Equals(id));
        }

        IQueryable<T> IRepository<T>.FindBy(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includes)
        {
            var result = Table.Where(predicate);
            if (includes.Any())
            {
                result = includes.Aggregate(result,
                    (current, include) 
                    => current.Include(include));
            }

            return result;
        }

        public async Task Add(T entity)
        {
            await Table.AddAsync(entity);
        }

        public async Task AddRange(IEnumerable<T> entities)
        {
            await Table.AddRangeAsync(entities);
        }              

        public async Task Update(T entitity)
        {
            Table.Update(entitity);
        }

        public async Task Remove(T entity)
        {
            Table.Remove(entity);
        }

        public async Task RemoveRange(IEnumerable<T> entitities)
        {
            Table.RemoveRange(entitities);
        }

        public void Dispose()
        {
            Db?.Dispose(); //Optimization of request after request fail
            GC.SuppressFinalize(this); //Clean current not used object
        }
    }
}
