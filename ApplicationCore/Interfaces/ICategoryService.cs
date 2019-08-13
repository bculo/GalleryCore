using ApplicationCore.Entities;
using ApplicationCore.Helpers.Pagination;
using System.Threading.Tasks;

namespace ApplicationCore.Interfaces
{
    public interface ICategoryService
    {
        Task<PaginationResult<Category>> GetCategories(int? page, string searchQuery);
    }
}
