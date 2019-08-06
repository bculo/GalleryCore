using System;

namespace ApplicationCore.Entities
{
    /// <summary>
    /// Image comments
    /// </summary>
    public class Comment : BaseEntity<long>
    {
        public string Description { get; set; }
        public DateTime Created { get; set; }
        public bool Show { get; set; }

        public string UserId { get; set; }
        public Uploader User { get; set; }

        public long ImageId { get; set; }
        public Image Image { get; set; }

        public void Remove()
        {
            Show = false;
        }

        public override string ToString() => Description;
    }
}
