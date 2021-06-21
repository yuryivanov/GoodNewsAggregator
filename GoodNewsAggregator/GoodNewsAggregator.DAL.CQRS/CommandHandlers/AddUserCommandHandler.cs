using AutoMapper;
using GoodNewsAggregator.DAL.Core;
using GoodNewsAggregator.DAL.Core.Entities;
using GoodNewsAggregator.DAL.CQRS.Commands;
using MediatR;
using Serilog;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace GoodNewsAggregator.DAL.CQRS.CommandHandlers
{
    public class AddUserCommandHandler : IRequestHandler<AddUserCommand, int>
    {
        private readonly GoodNewsAggregatorContext _dbContext;
        private readonly IMapper _mapper;

        public AddUserCommandHandler(GoodNewsAggregatorContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<int> Handle(AddUserCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var user = _mapper.Map<User>(request.User);

                await _dbContext.Users.AddAsync(user, cancellationToken);

                //How many records in db are changed, if 0 - request failed
                return await _dbContext.SaveChangesAsync(cancellationToken);
            }
            catch (Exception e)
            {
                Log.Error(e, "AddUserCommandHandler was not successful");
                throw;
            }          
        }
    }
}
