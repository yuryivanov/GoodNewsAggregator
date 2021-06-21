using GoodNewsAggregator.Core.DataTransferObjects;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoodNewsAggregator.DAL.CQRS.Commands
{
    public class AddCommentCommand : IRequest<int>
    {
        public CommentDto Comment { get; set; }
    }
}
