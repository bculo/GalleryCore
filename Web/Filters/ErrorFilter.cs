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
                RedirectOnException(context, "Error", "AppBadRequest");
            }

            if (context.Exception is InvalidRequest)
            {
                RedirectOnException(context, "Error", "AppBadRequest");
            }

            else if(context.Exception is InvalidUserException)
            {
                RedirectOnException(context, "Authentication", "Login");
            }

            base.OnException(context);
        }

        private void RedirectOnException(ExceptionContext context, string controllerName, string actionName)
        {
            context.Result = new RedirectToRouteResult(
                new RouteValueDictionary(new { controller = controllerName, action = actionName })
            );
        }
    }
}
