using ApplicationCore.Entities;
using ApplicationCore.Helpers.Pagination;
using ApplicationCore.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
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

            // Convert List<Category> to List<CategoryModel>
            var categoryModels = mapper.Map<List<CategoryModel>>(serviceResult.ResultSet);

            //Create pagination model
            IPaginationModel<CategoryModel> pagination = maker.FillPaginationModel(categoryModels, serviceResult.Options);

            //Display CategoryViewModel
            return View(new CategoryViewModel { SearchCategory = searchQuery ?? "", Pagination = pagination });
        }

    }
}