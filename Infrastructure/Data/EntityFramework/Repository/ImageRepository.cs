using ApplicationCore.Entities;
using ApplicationCore.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace Infrastructure.Data.EntityFramework.Repository
{
    public class ImageRepository : EfRepository<Image>, IImageRepository
    {
        public ImageRepository(ImageGalleryDbContext context, ISpecificationEvaluator<Image> evaluator) 
            : base(context, evaluator) { }

        public virtual async Task<Image> GetImageDetails(long imageId)
        {
            var result = await dbContext.Images
                .Where(item => item.Id == imageId)
                .Select(item => new Image
                {
                    Id = item.Id,
                    Description = item.Description,
                    Created = item.Created,
                    Url = item.Url,
                    Tags = item.Tags,
                    Comments = item.Comments
                        .OrderByDescending(comment => comment.Created)
                        .Select(comment => new Comment
                        {
                            User = comment.User,
                            Created = comment.Created,
                            Description = comment.Description,
                            Id = comment.Id,
                            UserId = comment.UserId
                        }).Take(3).ToList(),
                    User = item.User,
                    Likes = item.Likes,
                    CategoryId = item.CategoryId
                }).FirstOrDefaultAsync();

            return result;
        }
    }
}
