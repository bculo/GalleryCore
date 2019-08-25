using ApplicationCore.Entities;
using System.Linq;

namespace ApplicationCore.Specifications
{
    public class ImageSpecification : BaseSpecification<Image>
    {
        //Get all images with specific category
        public ImageSpecification(int categoryId)
            : base(i => i.CategoryId == categoryId) { }

        public ImageSpecification(int categoryId, int skip, int take, string searchQuery)
            : base(i => i.CategoryId == categoryId && i.Description.Contains(searchQuery))
        {
            ApplyPaging(skip, take);
        }

        public ImageSpecification(long id) : base(i => i.Id == id)
        {
            AddInclude(i => i.Likes);
            AddInclude(i => i.Tags);
            AddInclude(i => i.Comments);
            AddInclude(i => i.User);
        }
    }
}
