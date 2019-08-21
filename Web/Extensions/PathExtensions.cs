using System.Collections.Generic;
using System.IO;
using Web.Models.Category;

namespace Web.Extensions
{
    public static class PathExtensions
    {
        public static void GetFullCategoryPaths(this IEnumerable<CategoryModel> paths, string categoryImagesLocation)
        {
            foreach (var model in paths)
            {
                model.Url = Path.Combine(categoryImagesLocation, model.Url);
            }
        }

        public static void GetFullCategoryPath(this EditCategoryModel model, string categoryImagesLocation)
        {
            model.Url = Path.Combine(categoryImagesLocation, model.Url);
        }
    }
}
