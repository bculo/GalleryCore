using System.Collections.Generic;

namespace Web.Models.Image
{
    public class ImageDisplayModel
    {
        public string SearchCategory { get; set; } = string.Empty;
        public PaginationsProperties Pagination { get; set; }
        public IEnumerable<ImageBasicModel> Images;
    }
}
