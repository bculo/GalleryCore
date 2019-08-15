using Microsoft.AspNetCore.Http;
using System.IO;
using System.Threading.Tasks;

namespace Web.Extensions
{
    public static class IFormFileExtension
    {
        public static async Task SaveImageAsync(this IFormFile file, string uploadDirectory, string newImageName)
        {
            string finalPath = Path.Combine(uploadDirectory, newImageName);
            using (var stream = new FileStream(finalPath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }
        }
    }
}
