using System.ComponentModel.DataAnnotations;

namespace Web.Models.Authentication
{
    public class RegistrationModel
    {
        [Required]
        [StringLength(25)]
        public string UserName { get; set; }

        [Required]
        [DataType(DataType.EmailAddress)]
        [StringLength(25)]
        public string Email { get; set; }

        [Required]
        [StringLength(25)]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        [Display(Name = "Repeat password")]
        [Compare(nameof(Password), ErrorMessage = "Needs to be same as Password")]
        [DataType(DataType.Password)]
        public string RepeatPassword { get; set; }
    }
}
