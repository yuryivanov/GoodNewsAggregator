using GoodNewsAggregator.Core.DataTransferObjects;
using System;
using System.Collections.Generic;

namespace GoodNewsAggregator.Models.ViewModels.Comment
{
    public class CommentsListViewModel
    {
        public Guid NewsId { get; set; }
        public IEnumerable<CommentDto> Comments { get; set; }
    }
}
