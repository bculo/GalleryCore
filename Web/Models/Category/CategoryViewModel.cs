using ApplicationCore.Helpers.Pagination;

namespace Web.Models.Category
{
    public class CategoryViewModel
    {
        public string SearchCategory { get; set; } = string.Empty;
        public IPaginationModel<CategoryModel> Pagination { get; set; }
    }
}
