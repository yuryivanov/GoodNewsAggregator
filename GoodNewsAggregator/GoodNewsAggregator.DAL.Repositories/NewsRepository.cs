using GoodNewsAggregator.DAL.Core.Entities;
using GoodNewsAggregator.DAL.Core;
using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using GoodNewsAggregator.DAL.Repositories.Interfaces;


namespace GoodNewsAggregator.DAL.Repositories.Implementation
{
    public class NewsRepository : INewsRepository
    {
        private readonly GoodNewsAggregatorContext _context;
        public NewsRepository(GoodNewsAggregatorContext context)
        {
            _context = context;
        }

        public async Task Add(News news)
        {
            await _context.News.AddAsync(news);
            await _context.SaveChangesAsync();
        }

        public Task<News> GetNewsById(Guid id)
        {
            return _context.News.FirstOrDefaultAsync(news => news.Id.Equals(id));
        }

        public IQueryable<News> GetNews()
        {
            return _context.News;
        }

        public async Task Remove(Guid id)
        {
            var news = await GetNewsById(id);
            _context.News.Remove(news);
            await _context.SaveChangesAsync();
        }

        public async Task Update(News news)
        {
            _context.News.Update(news);
            await _context.SaveChangesAsync();
        }
    }    
}
