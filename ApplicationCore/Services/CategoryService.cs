using ApplicationCore.Entities;
using ApplicationCore.Interfaces;
using ApplicationCore.Specifications;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ApplicationCore.Services
{
    public class CategoryService : ICategoryService
    {
        protected readonly IAsyncRepository<Category> repository;

        public CategoryService(IAsyncRepository<Category> repository) => this.repository = repository;

        public virtual async Task<List<Category>> GetAllAsync()
        {
            return await repository.ListAllAsync();
        }

        public virtual async Task<List<Category>> GetAllWithAsync(string contains)
        {
            var specification = new CategorySpecification(contains);
            return await repository.ListAsync(specification);
        }
    }
}
