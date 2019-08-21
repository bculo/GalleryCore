using AutoMapper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Web.Configuration;
using Web.Filters;
using Web.Interfaces;

namespace Web.Controllers
{
    [ErrorFilter]
    public class ImageController : Controller, IControllerInformation
    {
        private readonly PaginationSettings settings;
        private readonly IHostingEnvironment environment;
        private readonly IMapper mapper;

        public string Name => nameof(ImageController);

        public ImageController(
            IOptions<PaginationSettings> settings,
            IHostingEnvironment environment,
            IMapper mapper)
        {
            this.settings = settings.Value;
            this.environment = environment;
            this.mapper = mapper;
        }

        [HttpGet]
        public IActionResult Index(int? page, string searchQuery)
        {
            return View();
        }

        [HttpGet]
        [Route("Index/{categoryId}")]
        public IActionResult Index(int id, int? page, string searchQuery)
        {
            return View();
        }
    }
}