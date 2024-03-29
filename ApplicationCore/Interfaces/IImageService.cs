﻿using ApplicationCore.Entities;
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
        Task<Image> GetImageByIdAsync(int imageId);
        Task<(int likes, int dislikes)> LikeImageAsync(long imageid, bool like, string userId);
        Task<IPaginationModel<Comment>> GetImageCommentsAsync(long imageId, int? page, int pageSize);
        Task CreateCommmentAsync(long imageId, string description, string userId);
    }
}
