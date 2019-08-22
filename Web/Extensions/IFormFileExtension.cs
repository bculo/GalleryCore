using Microsoft.AspNetCore.Http;
using System.IO;
using System.Linq;
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

        public static async Task UpdateImageAsync(this IFormFile file, string uploadDirectory, string newImageName, string oldImageName)
        {
            //Save new image
            await file.SaveImageAsync(uploadDirectory, newImageName);

            //Delete old image
            string oldImagePath = Path.Combine(uploadDirectory, oldImageName);
            if (File.Exists(oldImagePath))
            {
                File.Delete(oldImagePath);
            }
        }
    }
}
