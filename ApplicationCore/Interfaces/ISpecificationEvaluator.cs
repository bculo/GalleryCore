using System.Linq;

namespace ApplicationCore.Interfaces
{
    /// <summary>
    /// Specification evaluator
    /// </summary>
    /// <typeparam name="T">type</typeparam>
    public interface ISpecificationEvaluator<T> where T : class
    {
        IQueryable<T> GetQuery(IQueryable<T> build, ISpecification<T> specification, bool singleInstance);
    }
}
