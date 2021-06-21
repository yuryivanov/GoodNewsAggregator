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
    public class AddAccessTokenCommandHandler : IRequestHandler<AddAccessTokenCommand, int>
    {
        private readonly GoodNewsAggregatorContext _dbContext;
        private readonly IMapper _mapper;

        public AddAccessTokenCommandHandler(GoodNewsAggregatorContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<int> Handle(AddAccessTokenCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var user = _dbContext.Users.FirstOrDefault(x => x.Id.Equals(request.AccessTokenDto.UserId));

                var accessToken = new AccessToken()
                {
                    Id = request.AccessTokenDto.Id,
                    ExpireAt = request.AccessTokenDto.ExpireAt,
                    Token = request.AccessTokenDto.Token,
                    UserId = request.AccessTokenDto.UserId,
                    User = user
                };
                await _dbContext.AccessTokens.AddAsync(accessToken, cancellationToken);

                //How many records in db are changed, if 0 - request failed
                return await _dbContext.SaveChangesAsync(cancellationToken);
            }
            catch (Exception e)
            {
                Log.Error(e, "AddAccessTokenCommandHandler was not successful");
                throw;
            }            
        }
    }
}
