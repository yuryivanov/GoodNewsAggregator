using GoodNewsAggregator.DAL.Core.Entities;
using GoodNewsAggregator.DAL.Core;
using GoodNewsAggregator.DAL.Repositories.Interfaces;

namespace GoodNewsAggregator.DAL.Repositories.Implementation.Repositories
{
    public class RoleRepository : Repository<Role>, IRoleRepository
    {
        public RoleRepository(GoodNewsAggregatorContext context)
            : base(context)
        {           
        }
    }    
}
