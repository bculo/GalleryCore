using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;
using System;

namespace Web.Filters
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class)]
    public class UserLogedIn : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (UserAlreadyLogedIn(context.HttpContext))
            {
                context.Result = new RedirectToRouteResult(
                    new RouteValueDictionary(new { controller = "Home", action = "Index" })
                );
            }

            base.OnActionExecuting(context);
        }

        private bool UserAlreadyLogedIn(HttpContext context)
        {
            if (!context.User.Identity.IsAuthenticated)
            {
                return false;
            }

            return true;
        }
    }
}
