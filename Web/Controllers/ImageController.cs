using ApplicationCore.Entities;
using ApplicationCore.Extensions;
using ApplicationCore.Helpers.Pagination;
using ApplicationCore.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;
using Web.Configuration;
using Web.Extensions;
using Web.Filters;
using Web.Interfaces;
using Web.Models.Image;

namespace Web.Controllers
{
    [ErrorFilter]
    [Route("Image")]
    public class ImageController : Controller, IControllerInformation
    {
        private readonly PaginationSettings settings;
        private readonly IHostingEnvironment environment;
        private readonly IMapper mapper;
        private readonly IImageService service;

        public string Name => nameof(ImageController);

        public ImageController(
            IOptions<PaginationSettings> settings,
            IHostingEnvironment environment,
            IMapper mapper,
            IImageService service)
        {
            this.settings = settings.Value;
            this.environment = environment;
            this.mapper = mapper;
            this.service = service;
        }

        #region Display section

        [HttpGet]
        [Route("")]
        [Route("Index")]
        public virtual async Task<IActionResult> Index(int? page, string searchQuery)
        {
            var paginationResult = await service.GetImagesAsync(page, settings.ImagePageSize, searchQuery);
            var imageDisplayModel = PrepareModel(paginationResult, searchQuery);
            return View(imageDisplayModel);
        }

        [HttpGet]
        [Route("Index/{categoryId}")]
        public virtual async Task<IActionResult> Index([FromRoute] int categoryId, int? page, string searchQuery)
        {
            var paginationResult = await service.GetImagesByCategoryAsync(categoryId, page, settings.ImagePageSize, searchQuery);
            var imageDisplayModel = PrepareModel(paginationResult, searchQuery);
            return View(imageDisplayModel);
        }

        /// <summary>
        /// Prepare ImageDisplayModel
        /// </summary>
        /// <param name="paginationModel">IPaginationModel<Image> instnace</param>
        /// <param name="searchQuery">search queryy</param>
        /// <returns>ImageDisplayModel instance</returns>
        public ImageDisplayModel PrepareModel(IPaginationModel<Image> paginationModel, string searchQuery)
        {
            var imageDisplayModel = mapper.Map<ImageDisplayModel>(paginationModel);
            imageDisplayModel.SearchCategory = searchQuery;
            imageDisplayModel.Images.GetPaths(IHostingExtension.ImagesFolderDisplay);
            return imageDisplayModel;
        }

        #endregion

        #region Create section

        /*
        [HttpGet]
        [Route("Create")]
        public virtual async Task<IActionResult> Create() => View();

        [HttpGet]
        [ValidateModel]
        [Route("Create")]
        [ValidateAntiForgeryToken]
        public virtual async Task<IActionResult> Create(CreateImageModel model)
        {
            return View(model);
        }

        [HttpGet]
        [ValidateModel]
        [Route("Create")]
        [ValidateAntiForgeryToken]
        public virtual async Task<IActionResult> CreateAjax([FromForm] CreateImageModel model)
        {
            return View(model);
        }
        */

        #endregion
    }
}