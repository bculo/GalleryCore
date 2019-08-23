using ApplicationCore.Entities;

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
    }
}
