using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;
using System;

namespace Web.Filters
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class)]
    public class ErrorFilter : ExceptionFilterAttribute
    {
        public override void OnException(ExceptionContext context)
        {
            if (context.Exception is ArgumentNullException)
            {
                context.Result = new RedirectToRouteResult(
                    new RouteValueDictionary(new { controller = "Error", action = "AppBadRequest" })
                );
            }

            base.OnException(context);
        }
    }
}
