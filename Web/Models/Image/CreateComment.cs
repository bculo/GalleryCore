using System.ComponentModel.DataAnnotations;

namespace Web.Models.Image
{
    public class CreateComment
    {
        [Required]
        public long ImageId { get; set; }

        [Required]
        public string Description { get; set; }
    }
}
