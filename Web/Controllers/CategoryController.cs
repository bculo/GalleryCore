using ApplicationCore.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
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

        public CategoryController(ICategoryService service, IMapper mapper)
        {
            this.mapper = mapper;
            this.service = service;
        }

        [HttpGet]
        public virtual async Task<IActionResult> Index(int? page, string searchQuery)
        {
            var paginationModel = await service.GetCategories(page, searchQuery);

            var categoryView = mapper.Map<CategoryViewModel>(paginationModel);
            categoryView.SearchCategory = searchQuery ?? "";

            //Display CategoryViewModel
            return View(categoryView);
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