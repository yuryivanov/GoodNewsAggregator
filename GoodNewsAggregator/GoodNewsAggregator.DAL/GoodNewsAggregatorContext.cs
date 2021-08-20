using System;
using System.Collections.Generic;
using System.Text;
using GoodNewsAggregator.DAL.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace GoodNewsAggregator.DAL.Core
{
    public class GoodNewsAggregatorContext : DbContext
    {
        public DbSet<Comment> Comments { get; set; }
        public DbSet<News> News { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<RSS> RSS { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<AccessToken> AccessTokens { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }

        public GoodNewsAggregatorContext(DbContextOptions<GoodNewsAggregatorContext> options) : base(options) 
        { }
    }
}
