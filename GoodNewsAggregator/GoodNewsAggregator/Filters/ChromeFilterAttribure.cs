using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GoodNewsAggregator
{
    public class ChromeFilterAttribute : Attribute, IResourceFilter
    {
        //PASSING PARAMETERS:

        //private int _startHours;
        //private int _endHours;

        //public ChromeFilterAttribute(int startHours, int endHours)
        //{
        //    _startHours = startHours;
        //    _endHours = endHours;
        //}


        //This method will be executed after authorization filter and before executing action, action-filters,
        //exception filters & result-filters
        //1st entrance:
        public void OnResourceExecuting(ResourceExecutingContext context)
        {
            //How to use parameters from the constructor, example:
            //var dateTime = DateTime.UtcNow;

            //if (dateTime.Hour >= _startHours && dateTime.Hour <= _endHours)
            //{
            //    context.HttpContext.Response.Headers.Add("resource_filter", DateTime.UtcNow.ToString("t"));
            //}
            //else
            //{
            var userAgent = context.HttpContext.Request.Headers["User-Agent"].ToString();
            if (!userAgent.Contains("Chrome/"))
            {
                //Just return string:
                context.Result = new ContentResult()
                {
                    Content = "Извините, этот браузер не поддерживается сайтом. Пожалуйста, используйте Google Chrome браузер."
                };

                //Or another variant - Return View, it doesn't work, don't know why:
                //context.Result = new RedirectToActionResult("NotSupportedBrowserPage", "Home", null);
            }
            //}        
        }

        //2nd entrance
        //Another variant - will be executed only after all filters:
        public void OnResourceExecuted(ResourceExecutedContext context)
        {
            //context.Result = new RedirectToActionResult("Index","Home",null);
        }
    }
}
