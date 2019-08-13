using System.Collections.Generic;

namespace ApplicationCore.Helpers.Pagination
{
    public interface IPaginationModel<T>
    {
        int FirstPage { get; set; }
        int CurrentPage { get; set; }
        int LastPage { get; set; }
        int NumberOfItems { get; set; }
        int StartPageShow { get; set; }
        int EndPageShow { get; set; }

        IEnumerable<int> Pages { get; set; }
        IEnumerable<T> ResultsSet { get; set; }
    }
}
