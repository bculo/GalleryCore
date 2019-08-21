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
    public class CategoryService : ICategoryService
    {
        protected readonly IAsyncRepository<Category> repository;
        protected readonly IUniqueStringGenerator generator;
        protected readonly IPaginationMaker maker;

        public CategoryService(IAsyncRepository<Category> repository,
            IUniqueStringGenerator generator,
            IPaginationMaker maker)
        {
            this.repository = repository;
            this.generator = generator;
            this.maker = maker;
        }

        /// <summary>
        /// Categories pagination
        /// </summary>
        /// <param name="page">current page</param>
        /// <param name="searchQuery">search query</param>
        /// <param name="pageSize">pageSize parametar is fixed(user cant change it) 
        /// in this case so we dont need to check if pagesize is negative</param>
        /// <returns>Instnace of IPaginationModel</returns>
        public virtual async Task<IPaginationModel<Category>> GetCategoriesAsync(int? page, string searchQuery, int pageSize)
        {
            //Validate input
            int currentPage = page ?? 1;
            string search = searchQuery ?? string.Empty; 

            //Get number of instances from database that containts specific text
            var firstSpecification = new CategorySpecification(search);
            var numberOfCategories = await repository.CountAsync(firstSpecification);

            //Is requested page valid ?
            currentPage = maker.CheckPageLimits(currentPage, numberOfCategories, pageSize);

            //Get required instances from database
            int skip = maker.Skip(currentPage, pageSize);
            int take = pageSize;
            var secondSpecification = new CategorySpecification(skip, take, search);
            var listOfCategories = await repository.ListAsync(secondSpecification);

            //Return instance of IPaginationModel
            var pgOptions = new PaginationOptions(currentPage, numberOfCategories, pageSize);
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

            string newUniqueImageName = generator.GenerateUniqueString().Replace("-", "");
            string originalFileExtension = Path.GetExtension(imageName);
            var newCategoryInstance = new Category
            {
                Name = categoryName,
                Url = string.Concat(newUniqueImageName, originalFileExtension)
            };

            //TODO - name of category should be unique
            var (instance, error) = await repository.AddAsync(newCategoryInstance);
            if (!string.IsNullOrEmpty(error))
            {
                return serviceResult.BadRequest("Problem with adding new category to database, please try again");
            }

            return serviceResult.GoodRequest(instance.Url);
        }

        /// <summary>
        /// Get specific category by id
        /// </summary>
        /// <param name="id">id of category</param>
        /// <returns>Instance of Category</returns>
        public virtual async Task<Category> GetCategoryAsync(int id)
        {
            var repositoryResult = await repository.GetByIdAsync(id);
            if(repositoryResult == null)
            {
                throw new InvalidRequest("Selected category doesn't exist");
            }
            return repositoryResult;
        }
    }
}
