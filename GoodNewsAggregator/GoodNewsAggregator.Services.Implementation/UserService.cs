using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GoodNewsAggregator.Core.DataTransferObjects;
using GoodNewsAggregator.Core.Services.Interfaces;
using GoodNewsAggregator.DAL.Core.Entities;
using Microsoft.EntityFrameworkCore;
using GoodNewsAggregator.DAL.Repositories.Implementation;
using System.Text;
using System.Security.Cryptography;
using Serilog;

namespace GoodNewsAggregator.Services.Implementation
{
    public class UserService : IUserService
    {
        private readonly IUnitOfWork _unitOfWork;
        public UserService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<UserDto> FindUserById(Guid id)
        {
            try
            {
                var user = await _unitOfWork.Users.GetEntityById(id);

                return new UserDto()
                {
                    Id = user.Id,
                    Email = user.Email,
                    FullName = user.FullName,
                    PasswordHash = user.PasswordHash,
                    RoleId = user.RoleId
                };
            }
            catch (Exception e)
            {
                Log.Error(e, "FindUserById was not successful");
                throw;
            }            
        }

        public async Task<IEnumerable<UserDto>> FindUsers()
        {
            try
            {
                var users = await _unitOfWork.Users.FindBy(n
                        => n.Id.Equals(n.Id))
                    .ToListAsync();


                return users.Select(n => new UserDto()
                {
                    Id = n.Id,
                    Email = n.Email,
                    FullName = n.FullName,
                    PasswordHash = n.PasswordHash,
                    RoleId = n.RoleId
                }).ToList();
            }
            catch (Exception e)
            {
                Log.Error(e, "FindUsers was not successful");
                throw;
            }            
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
                var userRoleId =
                  (await _unitOfWork.Roles.FindBy(role => role.Name.Equals("User")).FirstOrDefaultAsync()).Id;

                await _unitOfWork.Users.Add(new User()
                {
                    Id = model.Id,
                    Email = model.Email,
                    PasswordHash = model.PasswordHash,
                    FullName = model.FullName,
                    RoleId = userRoleId
                });
                await _unitOfWork.SaveChangesAsync();
                return true;
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
                var user = await _unitOfWork.Users.FindBy(user => user.Email.Equals(email)).FirstOrDefaultAsync();

                if (user != null)
                {
                    return new UserDto
                    {
                        Id = user.Id,
                        Email = user.Email,
                        RoleId = user.RoleId,
                        PasswordHash = user.PasswordHash,
                        FullName = user.FullName
                    };
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

        public Task<int> AddAccessToken(AccessTokenDto accessTokenDto)
        {
            throw new NotImplementedException();
        }

        public Task<int> AddRefreshToken(RefreshTokenDto refreshTokenDto)
        {
            throw new NotImplementedException();
        }

        public Task<AccessTokenDto> GetAccessTokenByTokenString(string token)
        {
            throw new NotImplementedException();
        }

        public Task<RefreshTokenDto> GetRefreshTokenById(Guid id)
        {
            throw new NotImplementedException();
        }
    }
}
