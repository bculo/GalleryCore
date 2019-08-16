using ApplicationCore.Helpers.Pagination;
using System.Collections.Generic;
using Web.Extensions;

namespace Web.Models.Category
{
    public class CategoryViewModel
    {
        public string SearchCategory { get; set; } = string.Empty;
        public PaginationsProperties Pagination { get; set; }
        public IEnumerable<CategoryModel> Categories;
        public string CategoryPath = EnvironmentLocation.CategoryLocation;
    }
}
