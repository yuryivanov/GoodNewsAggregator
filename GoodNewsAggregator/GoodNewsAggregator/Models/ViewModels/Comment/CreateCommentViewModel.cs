using System;

namespace GoodNewsAggregator.Models.ViewModels.Comment
{
    public class CreateCommentViewModel
    {
        public Guid NewsId { get; set; }
        public string CommentText { get; set; }
    }
}
