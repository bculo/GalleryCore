using ApplicationCore.Entities;
using ApplicationCore.Helpers.Pagination;
using ApplicationCore.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using Web.Extensions;
using Web.Filters;
using Web.Models.Category;

namespace Web.Controllers
{
    public class CategoryController : Controller
    {
        private readonly IMapper mapper;
        private readonly ICategoryService service;
        private readonly IPaginationMaker<CategoryModel> maker;

        public CategoryController(ICategoryService service, IMapper mapper, IPaginationMaker<CategoryModel> maker)
        {
            this.mapper = mapper;
            this.service = service;
            this.maker = maker;
        }

        [HttpGet]
        public virtual async Task<IActionResult> Index(int? page, string searchQuery)
        {
            //Get pagination result for categories based on choosen page and search query
            PaginationResult<Category> serviceResult = await service.GetCategories(page, searchQuery);

            // From List<Category> to List<CategoryModel>
            var categoryModels = mapper.Map<List<CategoryModel>>(serviceResult.ResultSet);

            //Prepare pagination model
            IPaginationModel<CategoryModel> pagination = maker.PreparePaginationModel(categoryModels, serviceResult.Options);

            //Display CategoryViewModel
            return View(new CategoryViewModel { SearchCategory = searchQuery ?? "", Pagination = pagination });
        }

        //Only Moderator and Administrator roles
        [HttpGet]
        public virtual IActionResult Create() => View();

        [HttpPost]
        [ValidateModel]
        [ValidateAntiForgeryToken]
        public virtual async Task<IActionResult> Create(CreateCategoryModel model)
        {
            var serviceResult = await service.CreateNewCategoryAsync(model.Name, model.CategoryImage.FileName);

            if (!serviceResult.Success)
            {
                ModelState.FillWithErrors(serviceResult.Errors);
                return View(model);
            }

            await model.CategoryImage.SaveImageAsync(EnvironmentLocation.CategoryLocation, serviceResult.Result);

            return RedirectToAction(nameof(Index), ToString());
        }

        [HttpPost]
        [ValidateModel]
        [ValidateAntiForgeryToken]
        public virtual async Task<IActionResult> CreateAjax([FromForm] CreateCategoryModel model)
        {
            var serviceResult = await service.CreateNewCategoryAsync(model.Name, model.CategoryImage.FileName);

            if (!serviceResult.Success)
            {
                return Json(new { success = false, message = serviceResult.Errors[0] });
            }

            await model.CategoryImage.SaveImageAsync(EnvironmentLocation.CategoryLocation, serviceResult.Result);

            return Json(new { success = true, redirectAction = nameof(Index) });
        }

        public override string ToString() => nameof(CategoryController);
    }
}