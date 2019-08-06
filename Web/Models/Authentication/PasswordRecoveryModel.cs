using System.ComponentModel.DataAnnotations;

namespace Web.Models.Authentication
{
    public class PasswordRecoveryModel
    {
        [Required]
        [DataType(DataType.EmailAddress)]
        [Display(Name = "Email address")]
        public string Email { get; set; }
    }
}
