namespace ApplicationCore.Helpers.Pagination
{
    public class PaginationOptions
    {
        public int CurrentPage { get; }
        public int NumberOfItems { get; }
        public int PageSize { get; }
        public int ButtonToShow { get; set; }

        public PaginationOptions(int currentPage, int numberOfItems, int pageSize, int buttonToShow = 5)
        {
            CurrentPage = currentPage;
            NumberOfItems = numberOfItems;
            PageSize = pageSize;
            ButtonToShow = buttonToShow;
        }
    }
}
