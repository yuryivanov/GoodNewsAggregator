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
    public class GetUserByEmailQueryHandler : IRequestHandler<GetUserByEmailQuery, UserDto>
    {
        private readonly GoodNewsAggregatorContext _dbContext;
        private readonly IMapper _mapper; 

        public GetUserByEmailQueryHandler(GoodNewsAggregatorContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<UserDto> Handle(GetUserByEmailQuery request, CancellationToken cancellationToken)
        {
            try
            {
                return _mapper.Map<UserDto>(await _dbContext.Users.FirstOrDefaultAsync(user => user.Email.Equals(request.email), cancellationToken));
            }
            catch (Exception e)
            {
                Log.Error(e, "GetUserByEmailQueryHandler was not successful");
                throw;
            }
            
        }
    }
}
