using GoodNewsAggregator.DAL.Core.Entities;
using GoodNewsAggregator.DAL.Core;
using GoodNewsAggregator.DAL.Repositories.Interfaces;

namespace GoodNewsAggregator.DAL.Repositories.Implementation.Repositories
{
    public class NewsRepository : Repository<News>, INewsRepository
    {
        public NewsRepository(GoodNewsAggregatorContext context)
            : base(context)
        {           
        }
    }    
}
