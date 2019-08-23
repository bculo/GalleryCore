using ApplicationCore.Helpers.Path;
using System.Collections.Generic;
using System.IO;

namespace ApplicationCore.Extensions
{
    public static class PathUserExtension
    {
        public static void GetPaths(this IEnumerable<IPathUser> fileNames, string filesLocation)
        {
            foreach (var instance in fileNames)
            {
                instance.GetPaths(filesLocation);
            }
        }

        public static void GetPaths(this IPathUser fileName, string fileLocation)
        {
            fileName.Url = Path.Combine(fileLocation, fileName.Url);
        }
    }
}
