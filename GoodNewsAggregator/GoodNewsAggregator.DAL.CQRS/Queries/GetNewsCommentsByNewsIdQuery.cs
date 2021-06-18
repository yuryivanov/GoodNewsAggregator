using GoodNewsAggregator.Core.DataTransferObjects;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace GoodNewsAggregator.DAL.CQRS.Queries
{
    public class GetNewsCommentsByNewsIdQuery : IRequest<IEnumerable<CommentDto>>
    {
        public Guid? Id { get; set; }
    }
}
