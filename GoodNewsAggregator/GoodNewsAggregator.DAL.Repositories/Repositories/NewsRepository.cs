using GoodNewsAggregator.DAL.Core.Entities;
using GoodNewsAggregator.DAL.Core;
using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using GoodNewsAggregator.DAL.Repositories.Interfaces;
using System.Collections.Generic;

namespace GoodNewsAggregator.DAL.Repositories.Implementation
{
    public class NewsRepository : Repository<News>, INewsRepository
    {
        public NewsRepository(GoodNewsAggregatorContext context, DbSet<News> table)
            : base(context, table)
        {           
        }
    }    
}
