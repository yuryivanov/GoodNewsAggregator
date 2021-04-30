using System;

namespace GoodNewsAggregator.Core.DataTransferObjects
{
    public class UserDto
    {
        public Guid Id { get; set; }

        public string FullName { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }

        public Guid RoleId { get; set; }       
    }
}
