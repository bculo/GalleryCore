using System.Collections.Generic;

namespace Web.Models
{
    public class PaginationsProperties
    {
        public IEnumerable<int> Pages { get; set; }
        public int TotalItems { get; set; }
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
    }
}
