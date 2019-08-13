namespace ApplicationCore.Helpers.Pagination
{
    public class PaginationOptions
    {
        public int CurrentPage { get; }
        public int NumberOfItems { get; }
        public int PageSize { get; }

        public PaginationOptions(int currentPage, int numberOfItems, int pageSize)
        {
            CurrentPage = currentPage;
            NumberOfItems = numberOfItems;
            PageSize = pageSize;
        }
    }
}
