using ApplicationCore.Entities;
using ApplicationCore.Helpers.Pagination;
using ApplicationCore.Helpers.Service;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ApplicationCore.Interfaces
{
    public interface IImageService
    {
        Task<IPaginationModel<Image>> GetImagesAsync(int? page, int pageSize, string searchQuery);
        Task<IPaginationModel<Image>> GetImagesByCategoryAsync(int categoryId, int? page, int pageSize, string searchQuery);
        Task<ServiceResult<Image>> SaveNewImageAsync(int categoryId, string description, string imageFile,
            string userId, List<string> tags);
    }
}
