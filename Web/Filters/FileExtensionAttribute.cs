using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Web.Filters
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
    public class FileExtensionAttribute : ValidationAttribute
    {
        private IEnumerable<string> AllowedExtensions { get; set; }

        public FileExtensionAttribute(string extensions)
        {
            if (string.IsNullOrEmpty(extensions))
            {
                throw new ArgumentNullException(nameof(extensions));
            }

            AllowedExtensions = extensions.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                .ToList();
        }

        public override bool IsValid(object value)
        {
            var file = value as IFormFile;

            if(file == null)
            {
                return false;
            }

            var fileName = file.FileName;
            return AllowedExtensions.Any(exten => fileName.EndsWith(exten));
        }
    }
}
