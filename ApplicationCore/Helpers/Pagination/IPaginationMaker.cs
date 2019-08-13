using System.Collections.Generic;

namespace ApplicationCore.Helpers.Pagination
{
    /// <summary>
    /// iPaginationModel creator
    /// </summary>
    /// <typeparam name="Data"></typeparam>
    public interface IPaginationMaker<Data> where Data : class
    {
        IPaginationModel<Data> Pagination { get; }

        IPaginationModel<Data> FillPaginationModel(
             IEnumerable<Data> data,
             PaginationOptions options,
             int pageButtonsToShow = 5);

        IPaginationModel<Data> FillPaginationModel(
            PaginationResult<Data> paginationResult,
            int pageButtonsToShow = 5);
    }
}
