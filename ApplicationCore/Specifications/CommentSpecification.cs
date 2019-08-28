using ApplicationCore.Entities;

namespace ApplicationCore.Specifications
{
    public class CommentSpecification : BaseSpecification<Comment>
    {
        public CommentSpecification(long id)
            : base(item => item.ImageId == id) { }

        public CommentSpecification(long id, int skip, int take) 
            : base(item => item.ImageId == id)
        {
            AddInclude(item => item.User);
            ApplyPaging(skip, take);
        }
    }
}
