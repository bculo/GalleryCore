using ApplicationCore.Entities;
using ApplicationCore.Extensions;
using ApplicationCore.Helpers.Pagination;
using ApplicationCore.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Web.Configuration;
using Web.Extensions;
using Web.Filters;
using Web.Interfaces;
using Web.Models.Image;

namespace Web.Controllers
{
    [Authorize]
    [ErrorFilter]
    [Route("Image")]
    public class ImageController : Controller, IControllerInformation
    {
        private readonly PaginationSettings settings;
        private readonly IHostingEnvironment environment;
        private readonly IMapper mapper;
        private readonly IImageService service;

        public string Name => nameof(ImageController);

        public ImageController(
            IOptions<PaginationSettings> settings,
            IHostingEnvironment environment,
            IMapper mapper,
            IImageService service)
        {
            this.settings = settings.Value;
            this.environment = environment;
            this.mapper = mapper;
            this.service = service;
        }

        #region Display section

        [HttpGet]
        [Route("Index/{categoryId}")]
        public virtual async Task<IActionResult> Index([FromRoute] int categoryId, int? page, string searchQuery)
        {
            var paginationResult = await service.GetImagesByCategoryAsync(categoryId, page, settings.ImagePageSize, searchQuery);
            var imageDisplayModel = PrepareModel(paginationResult, searchQuery);
            imageDisplayModel.CategoryId = categoryId;
            return View(imageDisplayModel);
        }

        /// <summary>
        /// Prepare ImageDisplayModel
        /// </summary>
        /// <param name="paginationModel">IPaginationModel<Image> instnace</param>
        /// <param name="searchQuery">search queryy</param>
        /// <returns>ImageDisplayModel instance</returns>
        public ImageDisplayModel PrepareModel(IPaginationModel<Image> paginationModel, string searchQuery)
        {
            var imageDisplayModel = mapper.Map<ImageDisplayModel>(paginationModel);
            imageDisplayModel.SearchCategory = searchQuery;
            imageDisplayModel.Images.GetPaths(IHostingExtension.ImagesFolderDisplay);
            return imageDisplayModel;
        }

        #endregion

        #region Create section

        [HttpGet]
        [Route("Create/{categoryId}")]
        public virtual IActionResult Create([FromRoute] int categoryId)
        {
            return View(new CreateImageModel { CategoryId = categoryId });
        }


        [HttpPost]
        [ValidateModel]
        [ValidateAntiForgeryToken]
        [Route("Create/{categoryId}")]
        public virtual async Task<IActionResult> Create(CreateImageModel model)
        {
            string fileName = model.Image.FileName;
            List<string> tags = SeperateTags(model.Tags);
            string userId = User.GetUserId();
           
            var serviceResult = await service.SaveNewImageAsync(model.CategoryId, model.Description, fileName, userId, tags);
            if (serviceResult.Success)
            {
                await model.Image.SaveImageAsync(environment.GetFullImagesPath(), serviceResult.Result.Url);
                return RedirectToAction(nameof(Index), model.CategoryId);
            }

            ModelState.FillWithErrors(serviceResult.Errors);
            return View(model);
        }

        [HttpPost]
        [ValidateModel]
        [Route("CreateAjax")]
        [ValidateAntiForgeryToken]
        public virtual async Task<IActionResult> CreateAjax([FromForm] CreateImageModel model)
        {
            string fileName = model.Image.FileName;
            List<string> tags = SeperateTags(model.Tags);
            //string userId = User.GetUserId();
            string userId = "114068491279450791564"; //hardcoded user id 

            var serviceResult = await service.SaveNewImageAsync(model.CategoryId, model.Description, fileName, userId, tags);
            if (serviceResult.Success)
            {
                await model.Image.SaveImageAsync(environment.GetFullImagesPath(), serviceResult.Result.Url);
                return Json(new { success = true, redirectAction = nameof(Index) });
            }

            return Json(new { success = false, message = serviceResult.Errors[0] });
        }

        /// <summary>
        /// Convert one tag string into list of tags
        /// Input: "tag1, tag2, tag3" (string)
        /// Output: [tag1, tag2, tag3] (list)
        /// </summary>
        /// <param name="tags">string with tags</param>
        /// <param name="separator">spliting string with this separator</param>
        /// <returns>list of tags</returns>
        public List<string> SeperateTags(string tags, char separator = ',')
        {
            if (string.IsNullOrWhiteSpace(tags))
            {
                return new List<string>();
            }

            return tags.Split(new char[] { separator }, StringSplitOptions.RemoveEmptyEntries)
                .ToList();
        }

        #endregion

        #region Display selected image section

        [HttpGet]
        [Route("Detail/{imageId}")]
        public virtual async Task<IActionResult> Detail([FromRoute] int imageId)
        {
            var serviceResult = await service.GetImageByIdAsync(imageId);
            var viewModel = mapper.Map<ImageRichModel>(serviceResult);
            viewModel.GetPaths(IHostingExtension.ImagesFolderDisplay);
            return View(viewModel);
        }

        #endregion

        #region Image actions section

        [HttpPost]
        [ValidateModel]
        [Route("LikeAjax")]
        [ValidateAntiForgeryToken]
        public virtual async Task<IActionResult> LikeAjax(ImageLike model)
        {
            //TODO
            //Remove default value for Liked attribute and create new migration
            string userId = User.GetUserId();
            var (likes, dislikes) = await service.LikeImageAsync(model.Id, model.Like, userId);
            return Json(new { success = true, like = likes, dislike = dislikes });
        }

        [HttpGet]
        [Route("Comment/{imageId}")]
        public virtual async Task<IActionResult> Comment([FromRoute] long imageId, int page)
        {
            var serviceResult = await service.GetImageCommentsAsync(imageId, page, settings.CommentsPageSize);
            var viewModel = mapper.Map<CommentViewModel>(serviceResult);
            viewModel.ImageId = imageId;
            return View(viewModel);
        }

        [HttpPost]
        [ValidateModel]
        [ValidateAntiForgeryToken]
        [Route("Comment/{imageId}")]
        public virtual async Task<IActionResult> Comment(CreateComment model)
        {
            string userId = User.GetUserId();
            await service.CreateCommmentAsync(model.ImageId, model.Description, userId);

            //Get last page to show new comment
            var serviceResult = await service.GetImageCommentsAsync(model.ImageId, 20000000, settings.CommentsPageSize);
            var viewModel = mapper.Map<CommentViewModel>(serviceResult);
            viewModel.ImageId = model.ImageId;
            return View(viewModel);
        }

        #endregion
    }
}