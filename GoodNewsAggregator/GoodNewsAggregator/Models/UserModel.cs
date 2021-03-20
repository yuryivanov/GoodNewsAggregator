using System;
using System.Collections.Generic;
using System.Text;

namespace GoodNewsAggregator.Models
{
    public class UserModel
    {
        public Guid Id { get; set; }
        public string Login { get; set; }
        public string PasswordHash { get; set; }

        public int RoleId { get; set; }
        public RoleModel RoleModel { get; set; }
    }
}
