using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Linq;
using Web.Extensions;

namespace Web.Filters
{
    [AttributeUsage(AttributeTargets.Method)]
    public class ValidateModelAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (!context.ModelState.IsValid)
            {
                if (context.HttpContext.IsAjaxCall())
                {
                    context.Result = new BadRequestResult();
                }
                else
                {
                    var controller = context.Controller as Controller;
                    var model = context.ActionArguments?.Count > 0 ? context.ActionArguments.First().Value : null;
                    context.Result = (IActionResult)controller?.View(model) ?? new BadRequestResult();
                }
            }

            base.OnActionExecuting(context);
        }
    }
}
