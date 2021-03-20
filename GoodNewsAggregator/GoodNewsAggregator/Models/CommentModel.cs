using System;
using System.Collections.Generic;
using System.Text;

namespace GoodNewsAggregator.Models
{
    public class CommentModel
    {
        public Guid Id { get; set; }
        public string Text { get; set; }        
        public DateTime PublicationDate { get; set; }

        public Guid NewsId { get; set; }
        public NewsModel NewsModel { get; set; }

        public Guid UserId { get; set; }
        public UserModel UserModel { get; set; }
    }
}
