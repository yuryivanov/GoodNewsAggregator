using System;
using System.Text;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using GoodNewsAggregator.Models;
using Serilog;

namespace GoodNewsAggregator.HtmlHelpers
{
    public static class PaginationHelper
    {
        public static HtmlString CreatePagination(this IHtmlHelper html,
            PageInfo pageInfo,
            Func<int, string> pageUrl)
        {
            try
            {
                var sb = new StringBuilder();

                for (int i = 1; i <= pageInfo.TotalPages; i++)
                {
                    if (i == 1)
                    {
                        var str = $"<a class=\"btn btn-default\" href={pageUrl(i)}> {i.ToString()}</a>";

                        if (i == pageInfo.PageNumber)
                        {
                            str = $"<a class=\"btn selected btn btn-primary\" href={pageUrl(i)}> {i.ToString()}</a>";
                        }

                        sb.Append(str);

                        if (pageInfo.PageNumber >= 5)
                        {
                            sb.Append("<text id=\"paginationEllipsis\">...</text>");
                        }
                    }
                    else if (pageInfo.PageNumber - 2 <= i && i <= pageInfo.PageNumber + 2)
                    {
                        var str = $"<a class=\"btn btn-default\" href={pageUrl(i)}> {i.ToString()}</a>";

                        if (i == pageInfo.PageNumber)
                        {
                            str = $"<a class=\"btn selected btn btn-primary\" href={pageUrl(i)}> {i.ToString()}</a>";
                        }

                        sb.Append(str);
                    }
                    else if (i == pageInfo.TotalPages)
                    {
                        if (pageInfo.TotalPages >= pageInfo.PageNumber + 4)
                        {
                            sb.Append("<text id=\"paginationEllipsis\">...</text>");
                        }

                        var str = $"<a class=\"btn btn-default\" href={pageUrl(i)}> {i.ToString()}</a>";

                        if (i == pageInfo.PageNumber)
                        {
                            str = $"<a class=\"btn selected btn btn-primary\" href={pageUrl(i)}> {i.ToString()}</a>";
                        }

                        sb.Append(str);
                    }
                }

                return new HtmlString(sb.ToString());
            }
            catch (Exception e)
            {
                Log.Error(e, "CreatePagination was not successful");
                throw;
            }            
        }
    }
}