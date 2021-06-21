using GoodNewsAggregator.DAL.Core;
using GoodNewsAggregator.DAL.Core.Entities;
using GoodNewsAggregator.DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace GoodNewsAggregator.DAL.Repositories.Implementation.Repositories
{
    public abstract class Repository<T> : IRepository<T> where T : class, IBaseEntity
    {
        protected readonly GoodNewsAggregatorContext Db;
        protected readonly DbSet<T> Table;

        protected Repository(GoodNewsAggregatorContext context)
        {
            Db = context;
            Table = Db.Set<T>();
        }
        public async Task<T> GetEntityById(Guid id)
        {
            try
            {
                return await Table.FirstOrDefaultAsync(entity => entity.Id.Equals(id));
            }
            catch (Exception e)
            {
                Log.Error(e, "GetEntityById was not successful");
                throw;
            }            
        }

        public IQueryable<T> FindBy(Expression<Func<T, bool>> predicate,
              params Expression<Func<T, object>>[] includes)
        {
            try
            {
                var result = Table.Where(predicate);
                if (includes.Any())
                {
                    result = includes
                        .Aggregate(result,
                            (current, include)
                                => current.Include(include));
                }

                return result;
            }
            catch (Exception e)
            {
                Log.Error(e, "FindBy was not successful");
                throw;
            }            
        }

        public async Task Add(T entity)
        {
            try
            {
                await Table.AddAsync(entity);
            }
            catch (Exception e)
            {
                Log.Error(e, "Add was not successful");
                throw;
            }           
        }

        public async Task AddRange(IEnumerable<T> entities)
        {
            try
            {
                await Table.AddRangeAsync(entities);
            }
            catch (Exception e)
            {
                Log.Error(e, "AddRange was not successful");
                throw;
            }            
        }              

        public async Task Update(T entitity)
        {
            try
            {
                Table.Update(entitity);
            }
            catch (Exception e)
            {
                Log.Error(e, "Update was not successful");
                throw;
            }            
        }

        public async Task Remove(T entity)
        {
            try
            {
                Table.Remove(entity);
            }
            catch (Exception e)
            {
                Log.Error(e, "Remove was not successful");
                throw;
            }            
        }

        public async Task RemoveRange(IEnumerable<T> entitities)
        {
            try
            {
                Table.RemoveRange(entitities);
            }
            catch (Exception e)
            {
                Log.Error(e, "RemoveRange was not successful");
                throw;
            }            
        }

        public void Dispose()
        {
            try
            {
                Db?.Dispose(); //Optimization of request after request fail
                GC.SuppressFinalize(this); //Clean current not used object
            }
            catch (Exception e)
            {
                Log.Error(e, "Dispose was not successful");
                throw;
            }            
        }
    }
}
