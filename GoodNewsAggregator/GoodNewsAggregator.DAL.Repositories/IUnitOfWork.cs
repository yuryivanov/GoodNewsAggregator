using GoodNewsAggregator.DAL.Repositories.Interfaces;
using System.Threading.Tasks;

namespace GoodNewsAggregator.DAL.Repositories.Implementation
{
    public interface IUnitOfWork
    {
        INewsRepository News { get; }
        IRSSRepository RSS { get; }
        IUserRepository Users { get; }
        IRoleRepository Roles { get; }
        ICommentRepository Comments { get; }
        Task<int> SaveChangesAsync();
    }
}
