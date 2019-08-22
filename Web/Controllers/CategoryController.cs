using ApplicationCore.Entities;
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
    [Route("Category")]
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
            categoryView.Categories.GetFullCategoryPaths(IHostingExtension.CategoryFolderDisplay);

            //Display CategoryViewModel
            return View(categoryView);
        }

        #endregion

        #region Edit section

        [HttpGet]
        [Route("Edit/{categoryId}")]
        public virtual async Task<IActionResult> Edit(int categoryId)
        {
            //Get category
            var serviceResult = await service.GetCategoryAsync(categoryId);

            //Map Category to CategoryModel and set correct image url
            var categoryModel = mapper.Map<EditCategoryModel>(serviceResult);
            categoryModel.GetFullCategoryPath(IHostingExtension.CategoryFolderDisplay);

            //Display category model
            return View(categoryModel);
        }

        [HttpPost]
        [ValidateModel]
        [ValidateAntiForgeryToken]
        [Route("Edit/{categoryId}")]
        public virtual async Task<IActionResult> Edit(EditCategoryModel model)
        {
            var serviceResult = await service.UpdateCategoryAsync(model.Id, model.Name, model.CategoryImage?.FileName);
            if (serviceResult.Success)
            {
                //Prepare instances
                string oldImageUrl = serviceResult.Result.oldImageUrl;
                var category = serviceResult.Result.updatedCategory;

                //If category image is not null, new image is uploded
                await model.CategoryImage?.UpdateImageAsync(environment.GetFullCategoryPath(), category.Name, oldImageUrl);

                //Prepare success message 
                ViewBag.Message = "Category successfuly updated";

                //Prepare EditCategoryModel instance
                var categoryModel = mapper.Map<EditCategoryModel>(category);
                categoryModel.GetFullCategoryPath(IHostingExtension.CategoryFolderDisplay);

                return View(categoryModel);
            }

            //Display errors
            ModelState.FillWithErrors(serviceResult.Errors);
            return View(model);
        }

        [HttpPost]
        [ValidateModel]
        [ValidateAntiForgeryToken]
        [Route("EditAjax")]
        public virtual async Task<IActionResult> EditAjax(EditCategoryModel model)
        {
            var serviceResult = await service.UpdateCategoryAsync(model.Id, model.Name, model.CategoryImage?.FileName);
            if (serviceResult.Success)
            {
                //Prepare instances
                string oldImageUrl = serviceResult.Result.oldImageUrl;
                var category = serviceResult.Result.updatedCategory;

                //If category image is not null, new image is uploded
                await model.CategoryImage?.UpdateImageAsync(environment.GetFullCategoryPath(), category.Url, oldImageUrl);

                return Json(new { success = true, redirectAction = nameof(Index) });
            }

            return Json(new { success = false, message = serviceResult.Errors[0] });
        }

        #endregion

        #region Delete section

        #endregion

        #region Create section

        [HttpGet]
        [Route("Create")]
        public virtual IActionResult Create() => View();

        /// <summary>
        /// Normal post call
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateModel]
        [Route("Create")]
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
            await model.CategoryImage.SaveImageAsync(environment.GetFullCategoryPath(), serviceResult.Result.Url);

            return RedirectToAction(nameof(Index), Name);
        }

        /// <summary>
        /// Ajax call
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateModel]
        [Route("CreateAjax")]
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
            await model.CategoryImage.SaveImageAsync(environment.GetFullCategoryPath(), serviceResult.Result.Url);

            return Json(new { success = true, redirectAction = nameof(Index) });
        }

        #endregion
    }
}