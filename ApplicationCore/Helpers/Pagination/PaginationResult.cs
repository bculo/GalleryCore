using System.Collections.Generic;

namespace ApplicationCore.Helpers.Pagination
{
    /// <summary>
    /// Class represent pagination result
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class PaginationResult<T> where T : class
    {
        public PaginationOptions Options { get; }
        public IEnumerable<T> ResultSet { get; }

        public PaginationResult(int currentPage, int numberOfItems, IEnumerable<T> resultSet, int pageSize)
        {
            ResultSet = resultSet;
            Options = new PaginationOptions(currentPage, numberOfItems, pageSize);
        }
    }
}
