using System;
using GoodNewsAggregator.DAL.CQRS.Queries;
using GoodNewsAggregator.DAL.Core;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using GoodNewsAggregator.Core.DataTransferObjects;
using AutoMapper;
using MediatR;
using System.Threading;
using Serilog;

namespace GoodNewsAggregator.DAL.CQRS.QueryHandlers
{
    public class GetRefreshTokenByIdQueryHandler : IRequestHandler<GetRefreshTokenByIdQuery, RefreshTokenDto>
    {
        private readonly GoodNewsAggregatorContext _dbContext;
        private readonly IMapper _mapper;

        public GetRefreshTokenByIdQueryHandler(GoodNewsAggregatorContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<RefreshTokenDto> Handle(GetRefreshTokenByIdQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var entity = await _dbContext.RefreshTokens.FirstOrDefaultAsync(token => token.Id.Equals(request.Id), cancellationToken);

                return new RefreshTokenDto()
                {
                    Id = entity.Id,
                    Email = entity.Email,
                    ExpireAt = entity.ExpireAt,
                    Token = entity.Token,
                    UserId = entity.UserId
                };
            }
            catch (Exception e)
            {
                Log.Error(e, "GetRefreshTokenByIdQueryHandler was not successful");
                throw;
            }            
        }
    }
}
