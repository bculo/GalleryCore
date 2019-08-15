using System;
using System.IO;

namespace Web.Extensions
{
    public static class EnvironmentLocation
    {
        public static string CategoryLocation
        {
            get
            {
                string workingDirectory = Environment.CurrentDirectory;
                return Path.Combine(workingDirectory, "Files", "Category");
            }
        }

        public static string ImageLocation
        {
            get
            {
                string workingDirectory = Environment.CurrentDirectory;
                return Path.Combine(workingDirectory, "Files", "Images");
            }
        }
    }
}
