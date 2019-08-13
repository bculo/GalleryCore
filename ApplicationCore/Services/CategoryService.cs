using ApplicationCore.Entities;
using ApplicationCore.Helpers.Pagination;
using ApplicationCore.Interfaces;
using ApplicationCore.Specifications;
using System;
using System.Threading.Tasks;

namespace ApplicationCore.Services
{
    public class CategoryService : ICategoryService
    {
        protected readonly IAsyncRepository<Category> repository;
        protected readonly IPaginationChecker checker;

        public virtual int PageSize
        {
            get => 10;
        }

        public CategoryService(IAsyncRepository<Category> repository, IPaginationChecker checker)
        {
            this.repository = repository;
            this.checker = checker;
        }

        public virtual async Task<PaginationResult<Category>> GetCategories(int? page, string searchQuery)
        {
            int currentPage = page ?? 1;
            string search = searchQuery ?? string.Empty;

            var firstSpecification = new CategorySpecification(searchQuery ?? "");
            var numberOfCategories = await repository.CountAsync(firstSpecification);

            currentPage = checker.CheckPageLimits(currentPage, numberOfCategories, PageSize);

            var secondSpecification = new CategorySpecification((currentPage - 1) * PageSize, PageSize, searchQuery ?? "");
            var listOfCategories = await repository.ListAsync(secondSpecification);

            return new PaginationResult<Category>(currentPage, numberOfCategories, listOfCategories, PageSize);
        }
    }
}
