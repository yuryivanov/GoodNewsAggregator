using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GoodNewsAggregator.Core.DataTransferObjects;
using GoodNewsAggregator.Core.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using GoodNewsAggregator.DAL.Repositories.Implementation;
using Serilog;

namespace GoodNewsAggregator.Services.Implementation
{
    public class RoleService : IRoleService
    {
        private readonly IUnitOfWork _unitOfWork;
        public RoleService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<RoleDto>> FindRoles()
        {
            try
            {
                var roles = await _unitOfWork.Roles.FindBy(n
                         => n.Id.Equals(n.Id))
                     .ToListAsync();

                return roles.Select(n => new RoleDto()
                {
                    Id = n.Id,
                    Name = n.Name
                }).ToList();
            }
            catch (Exception e)
            {
                Log.Error(e, "FindRoles was not successful");
                throw;
            }            
        }

        public async Task<RoleDto> FindRoleById(Guid id)
        {
            try
            {
                var role = await _unitOfWork.Roles.GetEntityById(id);

                return new RoleDto()
                {
                    Id = role.Id,
                    Name = role.Name
                };
            }
            catch (Exception e)
            {
                Log.Error(e, "FindRoleById was not successful");
                throw;
            }            
        }
    }
}
