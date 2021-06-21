using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GoodNewsAggregator.Core.DataTransferObjects;
using GoodNewsAggregator.Core.Services.Interfaces;
using System.Text;
using System.Security.Cryptography;
using Serilog;
using MediatR;
using GoodNewsAggregator.DAL.CQRS.Commands;
using GoodNewsAggregator.DAL.CQRS.Queries;

namespace GoodNewsAggregator.Services.Implementation
{
    public class UserCqsService : IUserService
    {
        private readonly IMediator _mediator;
        public UserCqsService(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<UserDto> FindUserById(Guid id)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<UserDto>> FindUsers()
        {
            throw new NotImplementedException();
        }

        public string GetPasswordHash(string modelPassword)
        {
            try
            {
                var sha256 = new SHA256CryptoServiceProvider();
                var sha256data = sha256.ComputeHash(Encoding.UTF8.GetBytes(modelPassword));
                var hashedPassword = Encoding.UTF8.GetString(sha256data);
                return hashedPassword;
            }
            catch (Exception e)
            {
                Log.Error(e, "GetPasswordHash was not successful");
                throw;
            }           
        }

        public async Task<bool> RegisterUser(UserDto model)
        {
            try
            {
                var userRoleId = new Guid("07DEE73C-D974-46A7-AE2C-8367FCFBCB7F");

                var command = new AddUserCommand() { User = model };
                int res = await _mediator.Send(command);

                if (res > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }

            }
            catch (Exception e)
            {
                Log.Error(e, "RegisterUser was not successful");
                return false;
            }
        }

        public async Task<UserDto> GetUserByEmail(string email)
        {
            try
            {
                var query = new GetUserByEmailQuery() { email = email };
                var user = await _mediator.Send(query);

                if (user != null)
                {
                    return user;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception e)
            {
                Log.Error(e, "GetUserByEmail wasn't successfull");
                throw;
            }
        }

        public async Task<AccessTokenDto> GetAccessTokenByTokenString(string token)
        {
            try
            {
                var query = new GetAccessTokenByTokenStringQuery() { Token = token };
                var accessTokenDto = await _mediator.Send(query);

                if (accessTokenDto != null)
                {
                    return accessTokenDto;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception e)
            {
                Log.Error(e, "GetAccessTokenByTokenString wasn't successfull");
                throw;
            }
        }

        public async Task<RefreshTokenDto> GetRefreshTokenById(Guid id)
        {
            try
            {
                var query = new GetRefreshTokenByIdQuery() { Id = id };
                var refreshTokenDto = await _mediator.Send(query);

                if (refreshTokenDto != null)
                {
                    return refreshTokenDto;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception e)
            {
                Log.Error(e, "GetRefreshTokenById wasn't successfull");
                throw;
            }
        }

        public async Task<int> AddRefreshToken(RefreshTokenDto refreshTokenDto)
        {
            try
            {
                var command = new AddRefreshTokenCommand() { RefreshTokenDto = refreshTokenDto };
                return await _mediator.Send(command);
            }
            catch (Exception e)
            {
                Log.Error(e, "AddRefreshToken was not successful");
                throw;
            }            
        }

        public async Task<int> AddAccessToken(AccessTokenDto accessTokenDto)
        {
            try
            {
                var command = new AddAccessTokenCommand() { AccessTokenDto = accessTokenDto };
                return await _mediator.Send(command);
            }
            catch (Exception e)
            {
                Log.Error(e, "AddAccessToken was not successful");
                throw;
            }            
        }
    }
}
