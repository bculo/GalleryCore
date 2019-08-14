using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using Web.Filters;

namespace Web.Models.Category
{
    public class CreateCategoryModel
    {
        [Required]
        [Display(Name = "Category name")]
        public string Name { get; set; }

        [Required]
        [Display(Name = "Choose an image")]
        [DataType(DataType.Upload)]
        [FileExtensionAttribute("jpg,png,jpeg", ErrorMessage = "Wrong file type!")]
        public IFormFile CategoryImage { get; set; }
    }
}
