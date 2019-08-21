using Microsoft.AspNetCore.Hosting;
using System.IO;

namespace Web.Extensions
{
    public static class IHostingEnvironmentExtension
    {
        public static string GetFullCategoryPath(this IHostingEnvironment environment)
        {
            return Path.Combine(environment.ContentRootPath, CategoryFolder);
        }

        public static string CategoryFolder
        {
            get => Path.Combine("/", "Files", "Category");
        }

        public static string GetFullImagesPath(this IHostingEnvironment environment)
        {
            return Path.Combine(environment.ContentRootPath, ImagesFolder);
        }

        public static string ImagesFolder
        {
            get => Path.Combine("/", "Files", "Images");
        }
    }
}
