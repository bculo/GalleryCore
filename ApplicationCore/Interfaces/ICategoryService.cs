using ApplicationCore.Entities;
using ApplicationCore.Helpers.Pagination;
using ApplicationCore.Helpers.Service;
using System.Threading.Tasks;

namespace ApplicationCore.Interfaces
{
    public interface ICategoryService
    {
        Task<PaginationModel<Category>> GetCategories(int? page, string searchQuery);
        Task<ServiceResult<string>> CreateNewCategoryAsync(string categoryName, string imageName);
    }
}
