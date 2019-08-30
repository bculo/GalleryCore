using System.ComponentModel.DataAnnotations;

namespace Api.Models
{
    public class UserParams
    {
        [Required]
        public string UserName { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
