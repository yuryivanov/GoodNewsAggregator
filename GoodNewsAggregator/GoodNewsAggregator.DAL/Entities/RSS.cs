using System;
using System.Collections.Generic;
using System.Text;

namespace GoodNewsAggregator.DAL.Core.Entities
{
    public class RSS : IBaseEntity
    {
        public Guid Id { get; set; }

        public string Address { get; set; }

        public virtual ICollection<News> NewsCollection { get; set; }
    }
}
