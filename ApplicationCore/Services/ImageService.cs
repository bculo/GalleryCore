using ApplicationCore.Entities;
using ApplicationCore.Exceptions;
using ApplicationCore.Helpers.Images;
using ApplicationCore.Helpers.Pagination;
using ApplicationCore.Helpers.Service;
using ApplicationCore.Interfaces;
using ApplicationCore.Specifications;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ApplicationCore.Services
{
    public class ImageService : IImageService
    {
        protected readonly IAsyncRepository<Category> categoryRepo;
        protected readonly IAsyncRepository<Image> imageRepo;
        protected readonly IImageNameGenerator generator;
        protected readonly IPaginationMaker pgMaker;

        public ImageService(
            IAsyncRepository<Category> categoryRepo,
            IAsyncRepository<Image> imageRepo,
            IImageNameGenerator generator,
            IPaginationMaker maker)
        {
            this.categoryRepo = categoryRepo;
            this.imageRepo = imageRepo;
            this.generator = generator;
            this.pgMaker = maker;
        }

        /// <summary>
        /// Get all images
        /// </summary>
        /// <param name="page">current page</param>
        /// <param name="pageSize">number of items to show</param>
        /// <param name="searchQuery">search items by this criteria</param>
        /// <returns>IPaginationModel Instance</returns>
        public virtual async Task<IPaginationModel<Image>> GetImagesAsync(int? page, int pageSize, string searchQuery)
        {
            //Check for unwanted values
            int currentPage = page ?? 1;
            searchQuery = searchQuery ?? string.Empty;

            //Count number of iamge instances in repository
            int totalRecords = await imageRepo.CountAsync();

            //Check if variable currentPage valid
            currentPage = page ?? pgMaker.CheckPageLimits(currentPage, totalRecords, pageSize);

            //Get instances
            int skip = pgMaker.Skip(currentPage, pageSize);
            var instances = await imageRepo.ListPaginationAsync(skip, pageSize);

            //Prepare pagination model
            var pgOptions = new PaginationOptions(currentPage, totalRecords, pageSize);
            return pgMaker.PreparePaginationModel(instances, pgOptions);
        }

        /// <summary>
        /// Get images by category
        /// </summary>
        /// <param name="categoryId">Image category</param>
        /// <param name="page">current page</param>
        /// <param name="pageSize">number of items to show</param>
        /// <param name="searchQuery">search items by this criteria</param>
        /// <returns>IPaginationModel Instance</returns>
        public virtual async Task<IPaginationModel<Image>> GetImagesByCategoryAsync(int categoryId, int? page, int pageSize, string searchQuery)
        {
            //If category doesnt exist, throw InvalidRequest exception
            if (await categoryRepo.GetByIdAsync(categoryId) == null)
            {
                throw new InvalidRequest($"Category with id = {categoryId} doesnt exist");
            }

            //Check for unwanted values
            int currentPage = page ?? 1;
            searchQuery = searchQuery ?? string.Empty;

            //Count number of recoreds with specific category
            var imageCountSpecification = new ImageSpecification(categoryId);
            int totalRecords = await imageRepo.CountAsync(imageCountSpecification);

            //Check if variable currentPage valid
            currentPage = page ?? pgMaker.CheckPageLimits(currentPage, totalRecords, pageSize);

            //Get images
            int skip = pgMaker.Skip(currentPage, pageSize);
            var getImagesSpecification = new ImageSpecification(categoryId, skip, pageSize, searchQuery);
            var displayRecords = await imageRepo.ListAsync(getImagesSpecification);

            //Prepare pagination model
            var pgOptions = new PaginationOptions(currentPage, totalRecords, pageSize);
            return pgMaker.PreparePaginationModel(displayRecords, pgOptions);
        }

        public virtual async Task<ServiceResult<Image>> SaveNewImageAsync(int categoryId, string description, string imageFile, string userId, List<string> tags)
        {
            throw new NotImplementedException();
        }
    }
}
