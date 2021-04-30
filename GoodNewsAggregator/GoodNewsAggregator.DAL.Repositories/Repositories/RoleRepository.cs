using GoodNewsAggregator.DAL.Core.Entities;
using GoodNewsAggregator.DAL.Core;
using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using GoodNewsAggregator.DAL.Repositories.Interfaces;
using System.Collections.Generic;

namespace GoodNewsAggregator.DAL.Repositories.Implementation.Repositories
{
    public class RoleRepository : Repository<Role>, IRoleRepository
    {
        public RoleRepository(GoodNewsAggregatorContext context)
            : base(context)
        {           
        }
    }    
}
