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
    public class GetAccessTokenByTokenStringQueryHandler : IRequestHandler<GetAccessTokenByTokenStringQuery, AccessTokenDto>
    {
        private readonly GoodNewsAggregatorContext _dbContext;
        private readonly IMapper _mapper;

        public GetAccessTokenByTokenStringQueryHandler(GoodNewsAggregatorContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<AccessTokenDto> Handle(GetAccessTokenByTokenStringQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var entity = await _dbContext.AccessTokens.FirstOrDefaultAsync(token => token.Token.Equals(request.Token), cancellationToken);

                return new AccessTokenDto()
                {
                    Id = entity.Id,
                    ExpireAt = entity.ExpireAt,
                    Token = entity.Token,
                    UserId = entity.UserId
                };
            }
            catch (Exception e)
            {
                Log.Error(e, "GetAccessTokenByTokenStringQueryHandler was not successful");
                throw;
            }            
        }
    }
}
