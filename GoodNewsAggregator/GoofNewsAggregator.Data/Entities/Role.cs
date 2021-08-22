using System;
using System.Collections.Generic;
using System.Text;

namespace GoodNewsAggregator.DAL.Core.Entities
{
    public class Role : IBaseEntity
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public virtual ICollection<User> UserCollection { get; set; }  // 1 role for a lot of users
    }
}
