using System.Collections.Generic;

namespace ApplicationCore.Entities
{
    /// <summary>
    /// Image Uploader
    /// </summary>
    public class Uploader : BaseEntity<string>
    {
        public string UserName { get; set; }
        public string Email { get; set; }

        public ICollection<Image> UploadedImages { get; set; }
        public ICollection<Like> LikedImages { get; set; }
        public ICollection<Comment> Comments { get; set; }

        public override string ToString() => UserName;
    }
}
