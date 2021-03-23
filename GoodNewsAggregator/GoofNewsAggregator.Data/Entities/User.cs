using System;
using System.Collections.Generic;
using System.Text;

namespace GoodNewsAggregator.DAL.Core.Entities
{
    public class User : IBaseEntity
    {
        public Guid Id { get; set; }

        public string Login { get; set; }
        public string PasswordHash { get; set; }

        public int RoleId { get; set; }
        public virtual Role Role { get; set; }

        public virtual ICollection<Comment> CommentCollection { get; set; }
    }
}
