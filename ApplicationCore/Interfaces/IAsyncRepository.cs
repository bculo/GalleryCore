using System.Collections.Generic;
using System.Threading.Tasks;

namespace ApplicationCore.Interfaces
{
    /// <summary>
    /// Repository interface
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IAsyncRepository<T> where T : class
    {
        Task<T> GetByIdAsync(object id);
        Task<(T, string)> AddAsync(T entity);
        Task<string> UpdateAsync(T entity);
        Task DeleteAsync(T entity);
        Task<List<T>> ListAllAsync();
        Task<List<T>> ListPaginationAsync(int skip, int take);
        Task<List<T>> ListAsync(ISpecification<T> spec);
        Task<int> CountAsync();
        Task<int> CountAsync(ISpecification<T> spec);
        Task<T> GetSingleInstanceAsync(ISpecification<T> spec);
    }
}
