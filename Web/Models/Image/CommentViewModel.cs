using System.Collections.Generic;

namespace Web.Models.Image
{
    public class CommentViewModel
    {
        public long ImageId { get; set; }
        public PaginationsProperties Pagination { get; set; }
        public IEnumerable<ImageComment> Comments;
    }
}
