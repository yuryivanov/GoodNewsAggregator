using GoodNewsAggregator.Core.DataTransferObjects;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GoodNewsAggregator.Models.ViewModels.Comment
{
    public class CommentsListViewModel
    {
        public Guid NewsId { get; set; }
        public IEnumerable<CommentDto> Comments { get; set; }
    }
}
