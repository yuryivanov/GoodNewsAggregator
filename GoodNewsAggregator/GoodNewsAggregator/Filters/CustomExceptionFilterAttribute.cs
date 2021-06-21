using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Serilog;

namespace GoodNewsAggregator.Filters
{
    public class CustomExceptionFilterAttribute : Attribute, IExceptionFilter
    {
        public void OnException(ExceptionContext context)
        {
            try
            {
                var action = context.ActionDescriptor.DisplayName;
                var message = context.Exception.Message;
                var stackTrace = context.Exception.StackTrace;
                var httpRequest = context.HttpContext.Request;

                context.Result = new ViewResult()
                {
                    ViewName = "CustomError"
                };
            }
            catch (Exception e)
            {
                Log.Error(e, "AddCommentCommandHandler was not successful");
                throw;
            }            
        }
    }
}

//using GoodNewsAggregator.Models;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.AspNetCore.Mvc.Filters;
//using Microsoft.AspNetCore.Mvc.ViewFeatures;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;

//namespace GoodNewsAggregator.Filters
//{
//    //For ErrorViewModel
//    public class CustomExceptionFilterAttribute : Attribute, IExceptionFilter
//    {
//        public void OnException(ExceptionContext context)
//        {
//            //Name of Controller Method where exception appeared
//            var action = context.ActionDescriptor.DisplayName;

//            var message = context.Exception.Message;
//            var statusCode = context.HttpContext.Response.StatusCode;
//            //var stackTrace = context.Exception.StackTrace;
//            //var httpRequest = context.HttpContext.Request;           

//            //Get Model:
//            context.Result = new ViewResult()
//            {
//                ViewName = "CustomError",
//                //ViewData = new ViewDataDictionary(filterContext.Controller.ViewData)
//                //{
//                //    Model = // set the model
//            };

//            //var view = new ViewResult();
//            //view.ViewName = "CustomError";
//            //view.ViewData.Model = new CustomErrorViewModel() {StatusCode = statusCode, Message = message };

//            //context.Result = view;           
//        }
//    }
//}
