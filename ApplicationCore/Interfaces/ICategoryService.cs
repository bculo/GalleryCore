using ApplicationCore.Entities;
using ApplicationCore.Helpers.Images;
using ApplicationCore.Helpers.Pagination;
using ApplicationCore.Helpers.Service;
using System.Threading.Tasks;

namespace ApplicationCore.Interfaces
{
    public interface ICategoryService
    {
        Task<IPaginationModel<Category>> GetCategoriesAsync(int? page, string searchQuery, int pageSize);
        Task<ServiceResult<Category>> CreateNewCategoryAsync(string categoryName, string imageName);
        Task<Category> GetCategoryAsync(int id);
        Task<ServiceResult<(string oldImageUrl, Category updatedCategory)>> UpdateCategoryAsync(int id, string name, string fileName);
    }
}
