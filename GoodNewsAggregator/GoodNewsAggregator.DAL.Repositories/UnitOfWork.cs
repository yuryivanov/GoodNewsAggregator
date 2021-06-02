using GoodNewsAggregator.DAL.Core;
using GoodNewsAggregator.DAL.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GoodNewsAggregator.DAL.Repositories.Implementation
{
    public class UnitOfWork : IDisposable, IUnitOfWork
    {
        private readonly GoodNewsAggregatorContext _db;
        private readonly INewsRepository _newsRepository;
        private readonly IRSSRepository _rSSRepository;
        private readonly IUserRepository _userRepository;
        private readonly IRoleRepository _roleRepository;
        private readonly ICommentRepository _commentRepository;
               
        public UnitOfWork(GoodNewsAggregatorContext context,
            INewsRepository newsRepository,
            IRSSRepository rSSRepository,
            IUserRepository userRepository,
            IRoleRepository roleRepository,
            ICommentRepository commentRepository)
        {
            _newsRepository = newsRepository;
            _rSSRepository = rSSRepository;
            _userRepository = userRepository;
            _roleRepository = roleRepository;
            _commentRepository = commentRepository;
            _db = context;
        }

        INewsRepository IUnitOfWork.News
        {
            get
            {
                return _newsRepository;
            }
        }

        IRSSRepository IUnitOfWork.RSS => _rSSRepository;

        IUserRepository IUnitOfWork.Users => _userRepository;

        IRoleRepository IUnitOfWork.Roles => _roleRepository;

        ICommentRepository IUnitOfWork.Comments => _commentRepository;

        public async Task<int> SaveChangesAsync()
        {
            return await _db.SaveChangesAsync();
            //the method returns number of changed records
        }

        public void Dispose()
        {
            _db?.Dispose(); //Optimization of request after request fail
            GC.SuppressFinalize(this); //Clean current not used object
        }
    }
}
