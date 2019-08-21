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
using Web.Models.Category;

namespace Web.Controllers
{
    [ErrorFilter]
    public class CategoryController : Controller, IControllerInformation
    {
        private readonly IMapper mapper;
        private readonly ICategoryService service;
        private readonly IHostingEnvironment environment;
        private readonly PaginationSettings settings;

        public string Name => nameof(CategoryController);

        public CategoryController(ICategoryService service,
            IMapper mapper,
            IHostingEnvironment environment,
            IOptions<PaginationSettings> settings)
        {
            this.mapper = mapper;
            this.service = service;
            this.environment = environment;
            this.settings = settings.Value;
        }

        #region Display section

        [HttpGet]
        public virtual async Task<IActionResult> Index(int? page, string searchQuery)
        {
            //Get pagination result
            var paginationModel = await service.GetCategoriesAsync(page, searchQuery, settings.CategoryPageSize);

            //Prepare CategoryViewModel instance
            var categoryView = mapper.Map<CategoryViewModel>(paginationModel);
            categoryView.SearchCategory = searchQuery;
            categoryView.Categories.GetFullCategoryPaths(IHostingEnvironmentExtension.CategoryFolder);

            //Display CategoryViewModel
            return View(categoryView);
        }

        #endregion

        #region Edit section

        [HttpGet]
        [Route("/Edit/{categoryId}")]
        public virtual async Task<IActionResult> Edit(int categoryId)
        {
            //Get category
            var serviceResult = await service.GetCategoryAsync(categoryId);

            //Map Category to CategoryModel
            var categoryModel = mapper.Map<EditCategoryModel>(serviceResult);

            //Display category model
            return View(categoryModel);
        }

        #endregion

        #region Create section

        [HttpGet]
        public virtual IActionResult Create() => View();

        /// <summary>
        /// Normal post call
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateModel]
        [ValidateAntiForgeryToken]
        public virtual async Task<IActionResult> Create(CreateCategoryModel model)
        {
            //Create new category
            var serviceResult = await service.CreateNewCategoryAsync(model.Name, model.CategoryImage.FileName);

            if (!serviceResult.Success)
            {
                //Show error
                ModelState.FillWithErrors(serviceResult.Errors);
                return View(model);
            }

            //Save image to given location if everything alright
            await model.CategoryImage.SaveImageAsync(environment.GetFullCategoryPath(), serviceResult.Result);

            return RedirectToAction(nameof(Index), Name);
        }

        /// <summary>
        /// Ajax call
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateModel]
        [ValidateAntiForgeryToken]
        public virtual async Task<IActionResult> CreateAjax([FromForm] CreateCategoryModel model)
        {
            //Create new category
            var serviceResult = await service.CreateNewCategoryAsync(model.Name, model.CategoryImage.FileName);

            if (!serviceResult.Success)
            {
                //show error
                return Json(new { success = false, message = serviceResult.Errors[0] });
            }

            //Save image to given location
            await model.CategoryImage.SaveImageAsync(environment.GetFullCategoryPath(), serviceResult.Result);

            return Json(new { success = true, redirectAction = nameof(Index) });
        }

        #endregion
    }
}