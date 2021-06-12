using System;
using System.Collections.Generic;
using System.Text;

namespace GoodNewsAggregator.DAL.CQRS.Queries
{
    public class GetRssByIdQuery : IQuery
    {
        public Guid Id { get; set; }
    }
}
