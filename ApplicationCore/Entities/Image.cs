using System;
using System.Collections.Generic;

namespace ApplicationCore.Entities
{
    /// <summary>
    /// Uploded image
    /// </summary>
    public class Image : BaseEntity<long>
    {
        public string Description { get; set; }
        public DateTime Created { get; set; }
        public string Url { get; set; }

        public string UserId { get; set; }
        public Uploader User { get; set; }

        public int CategoryId { get; set; }
        public Category Category { get; set; }

        public IEnumerable<Tag> Tags { get; set; }
        public IEnumerable<Comment> Comments { get; set; }
        public IEnumerable<Like> Likes { get; set; }
    }
}
