using ApplicationCore.Entities;
using ApplicationCore.Exceptions;
using ApplicationCore.Helpers.Generator;
using ApplicationCore.Helpers.Pagination;
using ApplicationCore.Helpers.Service;
using ApplicationCore.Interfaces;
using ApplicationCore.Specifications;
using System;
using System.IO;
using System.Threading.Tasks;

namespace ApplicationCore.Services
{
    public class CategoryService : ICategoryService, IPaginationService
    {
        protected readonly IAsyncRepository<Category> repository;
        protected readonly IPaginationChecker checker;
        protected readonly IUniqueStringGenerator generator;
        protected readonly IPaginationMaker maker;

        public virtual int PageSize
        {
            get => 10;
        }

        public int Skip(int currentPage)
        {
            if (currentPage < 1)
            {
                throw new PaginationException("Current page needs to be greater then 0");
            }

            return (currentPage - 1) * PageSize;
        }

        public CategoryService(IAsyncRepository<Category> repository,
            IPaginationChecker checker,
            IUniqueStringGenerator generator,
            IPaginationMaker maker)
        {
            this.repository = repository;
            this.checker = checker;
            this.generator = generator;
            this.maker = maker;
        }

        /// <summary>
        /// Categories pagination
        /// </summary>
        /// <param name="page">current page</param>
        /// <param name="searchQuery">search query</param>
        /// <returns>Instnace of PaginationResult</returns>
        public virtual async Task<IPaginationModel<Category>> GetCategories(int? page, string searchQuery)
        {
            int currentPage = page ?? 1;
            string search = searchQuery ?? string.Empty;

            var firstSpecification = new CategorySpecification(searchQuery ?? "");
            var numberOfCategories = await repository.CountAsync(firstSpecification);

            currentPage = checker.CheckPageLimits(currentPage, numberOfCategories, PageSize); //is requested page valid ?

            var secondSpecification = new CategorySpecification(Skip(currentPage), PageSize, searchQuery ?? "");
            var listOfCategories = await repository.ListAsync(secondSpecification);

            var pgOptions = new PaginationOptions(currentPage, numberOfCategories, PageSize);
            return maker.PreparePaginationModel(listOfCategories, pgOptions);
        }

        /// <summary>
        /// Save new category to database and return category image name
        /// </summary>
        /// <param name="categoryName">name of category</param>
        /// <returns>unique image name</returns>
        public virtual async Task<ServiceResult<string>> CreateNewCategoryAsync(string categoryName, string imageName)
        {
            if (string.IsNullOrEmpty(categoryName))
            {
                throw new ArgumentNullException(nameof(categoryName));
            }

            if (string.IsNullOrEmpty(imageName))
            {
                throw new ArgumentNullException(nameof(imageName));
            }

            var serviceResult = new RequestResult<string>();

            var newCategoryInstance = new Category
            {
                Name = categoryName,
                Url = string.Concat(generator.GenerateUniqueString().Replace("-", ""), Path.GetExtension(imageName)),
            };

            var (instance, error) = await repository.AddAsync(newCategoryInstance);
            if (!string.IsNullOrEmpty(error))
            {
                return serviceResult.BadRequest("Problem with adding new category to database, please try again");
            }

            return serviceResult.GoodRequest(instance.Url);
        }
    }
}
