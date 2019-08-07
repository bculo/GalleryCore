using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers
{
    public class ErrorController : Controller
    {
        [HttpGet]
        public IActionResult Lost() => View();

        [HttpGet]
        public IActionResult AppBadRequest() => View();
    }
}