using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Web.Models.Authentication
{
    public class PasswordConfirmationModel
    {
        [HiddenInput(DisplayValue = false)]
        public string Token { get; set; }

        [HiddenInput(DisplayValue = false)]
        public string UserId { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [StringLength(maximumLength: 20, MinimumLength = 3)]
        public string Password { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Compare(nameof(Password))]
        public string RepeatPassword { get; set; }
    }
}
