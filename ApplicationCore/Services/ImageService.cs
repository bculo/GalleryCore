using ApplicationCore.Entities;
using ApplicationCore.Exceptions;
using ApplicationCore.Helpers.Images;
using ApplicationCore.Helpers.Pagination;
using ApplicationCore.Helpers.Service;
using ApplicationCore.Interfaces;
using ApplicationCore.Specifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApplicationCore.Services
{
    public class ImageService : IImageService
    {
        protected readonly IAsyncRepository<Category> categoryRepo;
        protected readonly IAsyncRepository<Comment> commentRepo;
        protected readonly IImageRepository imageRepo;
        protected readonly IImageNameGenerator generator;
        protected readonly IPaginationMaker pgMaker;

        public ImageService(
            IAsyncRepository<Category> categoryRepo,
            IImageRepository imageRepo,
            IImageNameGenerator generator,
            IPaginationMaker maker,
            IAsyncRepository<Comment> commentRepo)
        {
            this.categoryRepo = categoryRepo;
            this.imageRepo = imageRepo;
            this.generator = generator;
            this.pgMaker = maker;
            this.commentRepo = commentRepo;
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

        /// <summary>
        /// Create new image
        /// </summary>
        /// <param name="categoryId">image category</param>
        /// <param name="description">image description</param>
        /// <param name="imageFile">image name</param>
        /// <param name="userId">uploder id</param>
        /// <param name="tags">image tags</param>
        /// <returns>Instance of ServiceResult</returns>
        public virtual async Task<ServiceResult<Image>> SaveNewImageAsync(int categoryId, string description, string imageFile, string userId, List<string> tags)
        {
            //If category doesnt exist, throw InvalidRequest exception
            if (await categoryRepo.GetByIdAsync(categoryId) == null)
            {
                throw new InvalidRequest($"Category with id = {categoryId} doesnt exist");
            }

            if (string.IsNullOrEmpty(imageFile))
            {
                throw new ArgumentNullException(nameof(imageFile));
            }

            imageFile = generator.GetUniqueImageName(imageFile);

            var newImage = new Image
            {
                CategoryId = categoryId,
                Created = DateTime.Now,
                Tags = tags.Select(item => new Tag { Description = item }).ToList(),
                Description = description ?? "",
                Comments = new List<Comment>(),
                Likes = new List<Like>(),
                Url = imageFile,
                UserId = userId,
            };

            var serviceResult = new RequestResult<Image>();

            var (instnace, message) = await imageRepo.AddAsync(newImage);
            if (!string.IsNullOrEmpty(message))
            {
                return serviceResult.BadRequest(message);
            }
            else
            {
                return serviceResult.GoodRequest(instnace);
            }
        }

        /// <summary>
        /// Get specific image
        /// </summary>
        /// <param name="imageId">image id</param>
        /// <returns>Image instance</returns>
        public virtual async Task<Image> GetImageByIdAsync(int imageId)
        {
            var imageInstance = await imageRepo.GetImageDetails((long) imageId);
            if (imageInstance == null)
            {
                throw new InvalidRequest();
            }
            return imageInstance;
        }

        /// <summary>
        /// Like or dislike image
        /// </summary>
        /// <param name="imageid">image id</param>
        /// <param name="like">true for like, false for dislike</param>
        /// <param name="userId">user id</param>
        /// <returns>number of likes and number of dislieks</returns>
        public virtual async Task<(int likes, int dislikes)> LikeImageAsync(long imageid, bool like, string userId)
        {
            var imageInstance = await GetImageByIdAsync((int) imageid);

            imageInstance.Likes.Add(new Like
            {
                Created = DateTime.Now,
                Liked = like,
                UserId = userId,
            });

            await imageRepo.UpdateAsync(imageInstance);

            var likes = imageInstance.Likes.Count(item => item.Liked);
            var dislikes = imageInstance.Likes.Count(item => !item.Liked);
            return (likes, dislikes);
        }

        /// <summary>
        /// Comment pagination for choosen image 
        /// </summary>
        /// <param name="imageId"></param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public virtual async Task<IPaginationModel<Comment>> GetImageCommentsAsync(long imageId, int? page, int pageSize)
        {
            if(await imageRepo.GetByIdAsync(imageId) == null)
            {
                throw new InvalidRequest("Iamge doesnt exist");
            }

            var countSpec = new CommentSpecification(imageId);
            int numberOfInstances = await commentRepo.CountAsync(countSpec);
            int currentPage = pgMaker.CheckPageLimits(page, numberOfInstances, pageSize);
            int skip = pgMaker.Skip(currentPage, pageSize);

            var paginationSpec = new CommentSpecification(imageId, skip, pageSize);
            var commentInstances = await commentRepo.ListAsync(paginationSpec);

            var options = new PaginationOptions(currentPage, numberOfInstances, pageSize);
            return pgMaker.PreparePaginationModel<Comment>(commentInstances, options);
        }

        /// <summary>
        /// Add iamge comment
        /// </summary>
        /// <param name="imageId"></param>
        /// <param name="description"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public virtual async Task CreateCommmentAsync(long imageId, string description, string userId)
        {
            if (string.IsNullOrEmpty(description))
            {
                throw new ArgumentNullException(nameof(description));
            }

            var imageInstance = await imageRepo.GetImageWithComments(imageId);
            if(imageInstance == null)
            {
                throw new InvalidRequest("Iamge doesnt exist");
            }

            imageInstance.Comments.Add(new Comment
            {
                Created = DateTime.Now,
                Show = true,
                UserId = userId,
                Description = description,
            });

            await imageRepo.UpdateAsync(imageInstance);
        }
    }
}
