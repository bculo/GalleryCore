using System.Collections.Generic;

namespace Web.Models.Category
{
    public class CategoryViewModel
    {
        public string SearchCategory { get; set; } = string.Empty;
        public IEnumerable<CategoryModel> Categories { get; set; }
    }
}
