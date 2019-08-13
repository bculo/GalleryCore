using ApplicationCore.Entities;

namespace ApplicationCore.Specifications
{
    public class CategorySpecification : BaseSpecification<Category>
    {
        public CategorySpecification(int skip, int take, string contains)
            : base(i => i.Name.Contains(contains))
        {
            ApplyPaging(skip, take);
        }

        public CategorySpecification(string contains)
            : base(i => i.Name.Contains(contains))
        {
        }
    }
}
