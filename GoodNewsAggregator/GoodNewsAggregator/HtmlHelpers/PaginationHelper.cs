using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using GoodNewsAggregator.Models;

namespace GoodNewsAggregator.HtmlHelpers
{
    public static class PaginationHelper
    {
        public static HtmlString CreatePagination(this IHtmlHelper html,
            PageInfo pageInfo,
            Func<int, string> pageUrl)
        {
            var sb = new StringBuilder();
            for (int i = 1; i <= pageInfo.TotalPages; i++)
            {
                var str = $"<a class=\"btn btn-default\" href={pageUrl(i)}> {i.ToString()}</a>";

                if (i == pageInfo.PageNumber)
                {
                    str = $"<a class=\"btn selected btn btn-primary\" href={pageUrl(i)}> {i.ToString()}</a>";
                }
                sb.Append(str);
            }

            return new HtmlString(sb.ToString());
        }
    }
}