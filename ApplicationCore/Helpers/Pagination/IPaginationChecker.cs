namespace ApplicationCore.Helpers.Pagination
{
    public interface IPaginationChecker
    {
        int CheckPageLimits(int currentPage, int numberOfInstances, int pageSize, int lowestLimit = 1);
    }
}
