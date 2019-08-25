using System;
using System.ComponentModel.DataAnnotations;

namespace Web.Models.Image
{
    public class ImageComment
    {
        [Required]
        public long Id { get; set; }

        [Required]
        public string Description { get; set; }

        public DateTime Created { get; set; }

        public string UserName { get; set; }
    }
}
