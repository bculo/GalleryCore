using Microsoft.AspNetCore.Http;

namespace Infrastructure.Helpers.Http
{
    public abstract class HttpAccess
    {
        protected readonly IHttpContextAccessor accessor;

        protected HttpContext Http
        {
            get => accessor?.HttpContext;
        }

        protected HttpAccess(IHttpContextAccessor accessor)
        {
            this.accessor = accessor;
        }

        protected T GetSpecificServiceInstance<T>() where T : class
        {
            var instance = Http.RequestServices.GetService(typeof(T));
            return instance as T;
        }
    }
}
