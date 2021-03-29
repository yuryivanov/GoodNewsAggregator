using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GoodNewsAggregator.MiddlewareComponents
{
    public class TokenMiddleware
    {
        private readonly RequestDelegate _next;
        //For request work

        public TokenMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var token = context.Request.Query["token"];

            if (token != "fadsfe32fsdf")
            {
                context.Response.StatusCode = 401;
                await context.Response.WriteAsync("Invalid token");
            }
            else
            {
                await _next.Invoke(context);
            }
        }
    }
}
