using GoodNewsAggregator.Core.DataTransferObjects;
using System.Collections.Generic;

namespace GoodNewsAggregator.Models.ViewModels.News
{
    public class NewsListWithPaginationInfo
    {
        public IEnumerable<NewsDto> News { get; set; }
        public PageInfo PageInfo { get; set; }

        public string Email { get; set; }
        public string Name { get; set; }
        public string RoleName { get; set; } = "Гость";
    }
}
