using Microsoft.AspNetCore.Hosting;
using System.IO;

namespace Web.Extensions
{
    public static class IHostingExtension
    {
        /// <summary>
        /// Path for saving category images
        /// </summary>
        /// <param name="environment"></param>
        /// <returns></returns>
        public static string GetFullCategoryPath(this IHostingEnvironment environment)
        {
            return Path.Combine(environment.ContentRootPath, CategoryFolderSave);
        }

        public static string CategoryFolderDisplay
        {
            get => Path.Combine(string.Concat(Path.DirectorySeparatorChar, "Files"), "Category");
        }

        public static string CategoryFolderSave
        {
            get => Path.Combine("Files", "Category");
        }

        public static string GetFullImagesPath(this IHostingEnvironment environment)
        {
            return Path.Combine(environment.ContentRootPath, ImagesFolderSave);
        }

        public static string ImagesFolderDisplay
        {
            get => Path.Combine(string.Concat(Path.DirectorySeparatorChar, "Files"), "Images");
        }

        public static string ImagesFolderSave
        {
            get => Path.Combine("Files", "Images");
        }
    }
}
