using GoodNewsAggregator.Core.Services.Interfaces;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GoodNewsAggregator.Filters
{
    public class CheckDataFilterAttribute : Attribute, IAsyncActionFilter
    {
        private readonly INewsService _newsService;

        public CheckDataFilterAttribute(INewsService newsService)
        {
            _newsService = newsService;
        }
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var isContains = context.HttpContext.Request.QueryString.Value?.Contains("abc");
            if (isContains.GetValueOrDefault())
            {
                context.ActionArguments["hiddenId"] = 42;
                context.ActionArguments["newsId"] = _newsService.GetNewsByRSSId(null);
            }

            await next();
        }
    }
}
