using System;

namespace GoodNewsAggregator.Models
{
    public class PageInfo
    {
        public int PageNumber { get; set; }// current page
        public int PageSize { get; set; }//items on page
        public int TotalItems { get; set; }
        public int TotalPages => (int)Math.Ceiling((decimal)TotalItems / PageSize);
    }
}