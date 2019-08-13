using ApplicationCore.Interfaces;
using System.Collections.Generic;

namespace ApplicationCore.Helpers.Pagination
{
    public class PaginationModel<T> : IPaginationModel<T> where T : class
    {
        public int FirstPage { get; set; }
        public int CurrentPage { get; set; }
        public int LastPage { get; set; }
        public int NumberOfItems { get; set; }
        public int StartPageShow { get; set; }
        public int EndPageShow { get;  set; }

        public IEnumerable<int> Pages { get; set; }
        public IEnumerable<T> ResultsSet { get; set; }
    }
}
