﻿using ApplicationCore.Helpers.Path;

namespace Web.Models.Category
{
    public class CategoryModel : IPathUser
    {
        public int Id { get; set; }
        public string Url { get; set; }
        public string Name { get; set; }
    }
}
