using Microsoft.AspNetCore.Http;
using Serilog;
using System;
using System.Threading.Tasks;

namespace GoodNewsAggregator.MiddlewareComponents
{
    public class MyMiddleware
    {
        public readonly RequestDelegate _next;
        public MyMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            if (context.Request.Query.ContainsKey("first"))
            {
                context.Response.StatusCode = 200;
            }
            else
            {
                context.Response.StatusCode = 500;
            }            
        }
    }
}
