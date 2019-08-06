using ApplicationCore.Entities;
using ApplicationCore.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
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

        public CategoryController(ICategoryService service, IMapper mapper)
        {
            this.mapper = mapper;
            this.service = service;
        }

        //[Authorize]
        public async Task<IActionResult> Index()
        {
            return View(CreateCategoryViewModel(await service.GetAllAsync()));
        }

        //[Authorize]
        public async Task<IActionResult> QueryResult(string query)
        {
            var categoryViewModel = CreateCategoryViewModel(await service.GetAllWithAsync(query ?? ""), query);
            return View("Index", new { instance = categoryViewModel });
        }

        protected CategoryViewModel CreateCategoryViewModel(List<Category> categories, string query = "")
        {
            var categoryModels = mapper.Map<List<CategoryModel>>(categories ?? new List<Category>());
            var categoryViewModel = new CategoryViewModel
            {
                SearchCategory = query,
                Categories = categoryModels
            };
            return categoryViewModel;
        }
    }
}