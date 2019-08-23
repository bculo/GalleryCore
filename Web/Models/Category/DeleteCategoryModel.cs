using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Web.Models.Category
{
    public class DeleteCategoryModel
    {
        [Required]
        [HiddenInput(DisplayValue = false)]
        public int? Id { get; set; }

        [Required]
        [HiddenInput(DisplayValue = false)]
        public string Name { get; set; }
    }
}
