using System.ComponentModel.DataAnnotations;

namespace Web.Models.Image
{
    public class ImageLike
    {
        [Required]
        public long Id { get; set; }

        [Required]
        public bool Like { get; set; }
    }
}
