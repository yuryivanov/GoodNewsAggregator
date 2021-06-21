using GoodNewsAggregator.DAL.Core.Entities;
using GoodNewsAggregator.DAL.Core;
using GoodNewsAggregator.DAL.Repositories.Interfaces;

namespace GoodNewsAggregator.DAL.Repositories.Implementation.Repositories
{
    public class RSSRepository : Repository<RSS>, IRSSRepository
    {
        public RSSRepository(GoodNewsAggregatorContext context)
            : base(context)
        {           
        }
    }    
}
