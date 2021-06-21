using System;
using System.Collections.Generic;
using System.Text;

namespace GoodNewsAggregator.DAL.Core.Entities
{
    public class AccessToken : IBaseEntity
    {
        public Guid Id { get; set; }

        public string Token { get; set; }

        public DateTime ExpireAt { get; set; }

        public Guid UserId { get; set; }
        public virtual User User { get; set; }
    }
}
