using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using Web.Filters;

namespace Web.Models.Category
{
    public class EditCategoryModel
    {
        [Required]
        [HiddenInput(DisplayValue = false)]
        public int Id { get; set; }

        [Display(Name = "Current image")]
        [HiddenInput(DisplayValue = false)]
        public string Url { get; set; }

        [Required]
        [Display(Name = "Category name")]
        public string Name { get; set; }

        [Display(Name = "Choose new image for category")]
        [DataType(DataType.Upload)]
        [FileExtension("jpg,png,jpeg", nullValuePossible: true, ErrorMessage = "Wrong file type!")]
        public IFormFile CategoryImage { get; set; }
    }
}
