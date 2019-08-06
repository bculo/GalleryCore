using ApplicationCore.Entities;

namespace ApplicationCore.Specifications
{
    public class CategorySpecification : BaseSpecification<Category>
    {
        public CategorySpecification(string contains)
            : base(i => i.Name.Contains(contains, System.StringComparison.CurrentCultureIgnoreCase))
        {

        }
    }
}
