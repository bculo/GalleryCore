using ApplicationCore.Entities;
using ApplicationCore.Helpers.Pagination;
using ApplicationCore.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
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
            //LOGIC HERE

            return View(model);
        }

        [HttpPost]
        [ValidateModel]
        [ValidateAntiForgeryToken]
        public virtual IActionResult CreateAjax([FromForm] CreateCategoryModel model)
        {
            //LOGIC HERE

            return Json(new { success = true });
        }

    }
}