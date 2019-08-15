using Microsoft.AspNetCore.Http;

namespace Web.Extensions
{
    public static class HttpExtensions
    {
        public static bool IsAjaxCall(this HttpContext context)
        {
            return context.Request.Headers["x-requested-with"] == "XMLHttpRequest";
        }
    }
}
