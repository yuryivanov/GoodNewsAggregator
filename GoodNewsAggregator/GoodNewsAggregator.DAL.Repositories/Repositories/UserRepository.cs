using GoodNewsAggregator.DAL.Core.Entities;
using GoodNewsAggregator.DAL.Core;
using GoodNewsAggregator.DAL.Repositories.Interfaces;

namespace GoodNewsAggregator.DAL.Repositories.Implementation.Repositories
{
    public class UserRepository : Repository<User>, IUserRepository
    {
        public UserRepository(GoodNewsAggregatorContext context)
            : base(context)
        {           
        }
    }    
}
