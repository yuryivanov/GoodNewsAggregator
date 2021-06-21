using GoodNewsAggregator.DAL.Core.Entities;
using GoodNewsAggregator.DAL.Core;
using GoodNewsAggregator.DAL.Repositories.Interfaces;

namespace GoodNewsAggregator.DAL.Repositories.Implementation.Repositories
{
    public class CommentRepository : Repository<Comment>, ICommentRepository
    {
        public CommentRepository(GoodNewsAggregatorContext context)
            : base(context)
        {           
        }
    }    
}
