using ApplicationCore.Helpers.Path;
using System.Collections.Generic;

namespace Web.Models.Category
{
    public class CategoryViewModel
    {
        public string SearchCategory { get; set; } = string.Empty;
        public PaginationsProperties Pagination { get; set; }
        public IEnumerable<CategoryModel> Categories;
    }
}
