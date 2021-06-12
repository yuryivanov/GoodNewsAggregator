using System;

namespace GoodNewsAggregator.Core.DataTransferObjects
{
    public class RSSDto : IDtoModel
    {
        public Guid Id { get; set; }
        public string Address { get; set; }
    }
}
