using System;
using System.Collections.Generic;
using System.Text;

namespace GoodNewsAggregator.DAL.Core.Entities
{
    public class Comment : IBaseEntity
    {
        public Guid Id { get; set; }

        public string Text { get; set; }
        public DateTime PublicationDate { get; set; }


        public Guid NewsId { get; set; }
        public virtual News News { get; set; } //1 news has the comment

        public Guid UserId { get; set; }
        public virtual User User { get; set; } //1 user has the comment

        public string FullName { get; set; }
    }
}
