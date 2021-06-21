using AutoMapper;
using GoodNewsAggregator.DAL.Core;
using GoodNewsAggregator.DAL.Core.Entities;
using GoodNewsAggregator.DAL.CQRS.Commands;
using MediatR;
using Serilog;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace GoodNewsAggregator.DAL.CQRS.CommandHandlers
{
    public class AddRefreshTokenCommandHandler : IRequestHandler<AddRefreshTokenCommand, int>
    {
        private readonly GoodNewsAggregatorContext _dbContext;
        private readonly IMapper _mapper;

        public AddRefreshTokenCommandHandler(GoodNewsAggregatorContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<int> Handle(AddRefreshTokenCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var user = _dbContext.Users.FirstOrDefault(x => x.Id.Equals(request.RefreshTokenDto.UserId));

                var refreshToken = new RefreshToken()
                {
                    Id = request.RefreshTokenDto.Id,
                    Email = request.RefreshTokenDto.Email,
                    ExpireAt = request.RefreshTokenDto.ExpireAt,
                    Token = request.RefreshTokenDto.Token,
                    UserId = request.RefreshTokenDto.UserId,
                    User = user
                };
                await _dbContext.RefreshTokens.AddAsync(refreshToken, cancellationToken);

                //How many records in db are changed, if 0 - request failed
                return await _dbContext.SaveChangesAsync(cancellationToken);
            }
            catch (Exception e)
            {
                Log.Error(e, "AddRefreshTokenCommandHandler was not successful");
                throw;
            }            
        }
    }
}
