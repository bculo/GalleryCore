using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using Web.Filters;

namespace Web.Models.Category
{
    public class EditCategoryModel
    {
        [HiddenInput(DisplayValue = false)]
        public string Id { get; set; }

        [HiddenInput(DisplayValue = false)]
        public string Url { get; set; }

        [Required]
        [Display(Name = "Category name")]
        public string Name { get; set; }

        [Required]
        [Display(Name = "Choose an image")]
        [DataType(DataType.Upload)]
        [FileExtension("jpg,png,jpeg", ErrorMessage = "Wrong file type!")]
        public IFormFile CategoryImage { get; set; }
    }
}
