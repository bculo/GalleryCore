using ApplicationCore.Exceptions;
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
                RedirectOnException(context.Result, "Error", "AppBadRequest");
            }
            else if(context.Exception is InvalidUserException)
            {
                RedirectOnException(context.Result, "Authentication", "Login");
            }

            base.OnException(context);
        }

        private void RedirectOnException(IActionResult result, string controllerName, string actionName)
        {
            result = new RedirectToRouteResult(
                new RouteValueDictionary(new { controller = "Error", action = "AppBadRequest" })
            );
        }
    }
}
