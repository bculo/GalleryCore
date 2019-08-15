namespace ApplicationCore.Interfaces
{
    public interface IPaginationService
    {
        int PageSize { get; }
        int Skip(int currentPage);
    }
}
