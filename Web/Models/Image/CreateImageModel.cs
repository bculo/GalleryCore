using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using Web.Filters;

namespace Web.Models.Image
{
    public class CreateImageModel
    {
        [Required]
        [HiddenInput(DisplayValue = false)]
        public int CategoryId { get; set; }

        public string Description { get; set; }

        [Display(Name = "Tags (seperate tags using ,)")]
        public string Tags { get; set; }

        [Required]
        [DataType(DataType.Upload)]
        [FileExtension("jpg,jpeg,png", ErrorMessage = "Wrong file type!")]
        public IFormFile Image { get; set; }
    }
}
