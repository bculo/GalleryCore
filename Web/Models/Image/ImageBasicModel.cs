using ApplicationCore.Helpers.Path;

namespace Web.Models.Image
{
    public class ImageBasicModel : IPathUser
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public string Url { get; set; }
    }
}
