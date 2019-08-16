using System.Collections.Generic;

namespace ApplicationCore.Helpers.Pagination
{
    /// <summary>
    /// iPaginationModel creator
    /// </summary>
    /// <typeparam name="Data"></typeparam>
    public interface IPaginationMaker
    {
        PaginationModel<DataType> PreparePaginationModel<DataType>(
            IEnumerable<DataType> data,
            PaginationOptions options)
            where DataType : class;
    }
}
