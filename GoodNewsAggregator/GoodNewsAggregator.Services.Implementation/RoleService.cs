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
            var roles = await _unitOfWork.Roles.FindBy(n
                        => n.Id.Equals(n.Id))
                    .ToListAsync();
           
            return roles.Select(n => new RoleDto()
            {
                Id = n.Id,
                Name = n.Name
            }).ToList();
        }

        public async Task<RoleDto> FindRoleById(Guid id)
        {
            var role = await _unitOfWork.Roles.GetEntityById(id);

            return new RoleDto()
            {
                Id = role.Id,
                Name = role.Name
            };
        }
    }
}
