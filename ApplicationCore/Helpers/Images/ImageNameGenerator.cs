using ApplicationCore.Helpers.Generator;
using System.IO;

namespace ApplicationCore.Helpers.Images
{
    public class ImageNameGenerator : IImageNameGenerator
    {
        protected readonly IUniqueStringGenerator generator;

        public ImageNameGenerator(IUniqueStringGenerator unique) => generator = unique;

        public string GetUniqueImageName(string fileNameWithExtension)
        {
            string newUniqueImageName = generator.GenerateUniqueString().Replace("-", "");
            string originalFileExtension = System.IO.Path.GetExtension(fileNameWithExtension);
            return string.Concat(newUniqueImageName, originalFileExtension);
        }
    }
}
