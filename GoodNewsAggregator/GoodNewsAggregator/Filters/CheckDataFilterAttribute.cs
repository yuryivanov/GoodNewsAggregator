using GoodNewsAggregator.Core.Services.Interfaces;
using Microsoft.AspNetCore.Mvc.Filters;
using Serilog;
using System;
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
            try
            {
                var isContains = context.HttpContext.Request.QueryString.Value?.Contains("abc");
                if (isContains.GetValueOrDefault())
                {
                    context.ActionArguments["hiddenId"] = 42;
                    context.ActionArguments["newsId"] = _newsService.GetNewsByRSSId(null);
                }

                await next();
            }
            catch (Exception e)
            {
                Log.Error(e, "OnActionExecutionAsync was not successful");
                throw;
            }            
        }
    }
}
