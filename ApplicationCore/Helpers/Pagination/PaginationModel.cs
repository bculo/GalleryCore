using System.Collections.Generic;

namespace ApplicationCore.Helpers.Pagination
{
    public class PaginationModel<T> : IPaginationModel<T>
    {
        public int TotalItems { get; set; }
        public int CurrentPage { get; set; }
        public int PageSize { get; set; }
        public int TotalPages { get; set; }
        public int StartPage { get; set; }
        public int EndPage { get; set; }

        public IEnumerable<int> Pages { get; set; }

        public IEnumerable<T> Data { get; set; }
    }
}
