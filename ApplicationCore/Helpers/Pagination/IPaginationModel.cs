using System.Collections.Generic;

namespace ApplicationCore.Helpers.Pagination
{
    public interface IPaginationModel<T>
    {
        int TotalItems { get; set; }
        int CurrentPage { get; set; }
        int PageSize { get; set; }
        int TotalPages { get; set; }
        int StartPage { get; set; }
        int EndPage { get; set; }

        IEnumerable<int> Pages { get; set; }
        IEnumerable<T> Data { get; set; }
    }
}
