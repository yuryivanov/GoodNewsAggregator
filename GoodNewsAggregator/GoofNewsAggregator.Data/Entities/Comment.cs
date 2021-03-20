using System;
using System.Collections.Generic;
using System.Text;

namespace GoodNewsAggregator.DAL.Core.Entities
{
    public class Comment
    {
        public Guid Id { get; set; }

        public string Text { get; set; }
        public DateTime PublicationDate { get; set; }


        public Guid NewsId { get; set; }
        public virtual News News { get; set; }

        public Guid UserId { get; set; }
        public virtual User User { get; set; }       
    }
}
