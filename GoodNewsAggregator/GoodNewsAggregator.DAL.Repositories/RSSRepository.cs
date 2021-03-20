using GoodNewsAggregator.DAL.Core.Entities;
using GoodNewsAggregator.DAL.Core;
using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using GoodNewsAggregator.DAL.Repositories.Interfaces;

namespace GoodNewsAggregator.DAL.Repositories.Implementation
{
    public class RSSRepository : IRSSRepository
    {
        private readonly GoodNewsAggregatorContext _context;
        public RSSRepository(GoodNewsAggregatorContext context)
        {
            _context = context;
        }

        public async Task Add(RSS rss)
        {
            await _context.RSS.AddAsync(rss);
            await _context.SaveChangesAsync();
        }

        public Task<RSS> GetRSSById(Guid id)
        {
            return _context.RSS.FirstOrDefaultAsync(rss => rss.Id.Equals(id));
        }

        public IQueryable<RSS> GetRSS()
        {
            return _context.RSS;
        }

        public async Task Remove(Guid id)
        {
            var rss = await GetRSSById(id);
            _context.RSS.Remove(rss);
            await _context.SaveChangesAsync();
        }

        public async Task Update(RSS rss)
        {
            _context.RSS.Update(rss);
            await _context.SaveChangesAsync();
        }
    }
}

