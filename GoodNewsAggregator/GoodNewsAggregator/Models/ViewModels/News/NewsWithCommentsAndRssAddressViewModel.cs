using GoodNewsAggregator.Core.DataTransferObjects;
using System;
using System.Collections.Generic;

namespace GoodNewsAggregator.Models.ViewModels.News
{
    public class NewsWithCommentsAndRssAddressViewModel
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Address { get; set; }
        public string Description { get; set; }
        public DateTime? PublicationDate { get; set; }
        public string Text { get; set; }
        public double? GoodnessCoefficient { get; set; }

        public Guid? RSS_Id { get; set; }
        public string RSSAddress { get; set; }

        public IEnumerable<CommentDto> Comments { get; set; }
    }
}
