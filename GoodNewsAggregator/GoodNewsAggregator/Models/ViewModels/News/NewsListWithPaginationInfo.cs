using GoodNewsAggregator.Core.DataTransferObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GoodNewsAggregator.Models.ViewModels.News
{
    public class NewsListWithPaginationInfo
    {
        public IEnumerable<NewsDto> News { get; set; }
        public PageInfo PageInfo { get; set; }
    }
}
