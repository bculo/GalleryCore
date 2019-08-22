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
        private bool NullPossible { get; set; } = false;

        public FileExtensionAttribute(string extensions, bool nullValuePossible = false)
        {
            NullPossible = nullValuePossible;
            SetExtensions(extensions);
        }

        /// <summary>
        /// Set possible extensions
        /// </summary>
        /// <param name="extensions">extension string (jpg, jpeg, ...)</param>
        private void SetExtensions(string extensions)
        {
            if (string.IsNullOrEmpty(extensions))
            {
                throw new ArgumentNullException(nameof(extensions));
            }

            AllowedExtensions = extensions
                .Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                .ToList();
        }

        /// <summary>
        /// Check if object is valid
        /// </summary>
        /// <param name="value">object</param>
        /// <returns>return true if object is valid</returns>
        public override bool IsValid(object value)
        {
            var file = value as IFormFile;

            if(file == null && !NullPossible)
            {
                return false;
            }

            if(file == null && NullPossible)
            {
                return true;
            }

            var fileName = file.FileName;
            return AllowedExtensions.Any(exten => fileName.EndsWith(exten));
        }
    }
}
