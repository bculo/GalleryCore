using System;

namespace ApplicationCore.Entities
{
    /// <summary>
    /// Image like / dislike
    /// </summary>
    public class Like
    {
        public bool Liked { get; set; }
        public DateTime Created { get; set; }

        public string UserId { get; set; }
        public Uploader User { get; set; }

        public long ImageId { get; set; }
        public Image Image { get; set; }

        public void ChangeState()
        {
            if (Liked)
            {
                Liked = false;
            }
            else
            {
                Liked = true;
            }
        }
    }
}
