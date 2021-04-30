using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GoodNewsAggregator.Core.DataTransferObjects;
using GoodNewsAggregator.Core.Services.Interfaces;
using GoodNewsAggregator.DAL.Core;
using GoodNewsAggregator.DAL.Core.Entities;
using GoodNewsAggregator.DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using GoodNewsAggregator.DAL.Repositories.Implementation;
using System.Xml;
using System.ServiceModel.Syndication;
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

        public async Task<IEnumerable<UserDto>> FindUsers()
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

        public string GetPasswordHash(string modelPassword)
        {
            var sha256 = new SHA256CryptoServiceProvider();
            var sha256data = sha256.ComputeHash(Encoding.UTF8.GetBytes(modelPassword));
            var hashedPassword = Encoding.UTF8.GetString(sha256data);
            return hashedPassword;
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
                Log.Error(e, "Register was not successful");
                return false;
            }
        }

        public async Task<UserDto> GetUserByEmail(string email)
        {
            try
            {
                var user = await _unitOfWork.Users.FindBy(user => user.Email.Equals(email)).FirstOrDefaultAsync();
                return new UserDto
                {
                    Id = user.Id,
                    Email = user.Email,
                    RoleId = user.RoleId,
                    PasswordHash = user.PasswordHash,
                    FullName = user.FullName
                };
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}
