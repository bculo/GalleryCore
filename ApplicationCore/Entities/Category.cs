using System.Collections.Generic;

namespace ApplicationCore.Entities
{
    /// <summary>
    /// Image categories
    /// </summary>
    public class Category : BaseEntity<int>
    {
        public string Name { get; set; }
        public string Url { get; set; } //category image

        public ICollection<Image> Images { get; set; }

        public override string ToString() => Name;
    }
}
