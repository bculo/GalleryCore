using ApplicationCore.Helpers.Path;
using System;
using System.Collections.Generic;

namespace Web.Models.Image
{
    public class ImageRichModel : IPathUser
    {
        public long Id { get; set; }
        public int CategoryId { get; set; }
        public string Description { get; set; }
        public DateTime Created { get; set; }
        public string UploderName { get; set; }
        public string Url { get; set; }
        public List<string> Tags { get; set; }
        public int Likes { get; set; }
    }
}
