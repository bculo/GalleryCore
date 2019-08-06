using ApplicationCore.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace Infrastructure.Data.EntityFramework
{
    /// <summary>
    /// Specification evaluator for entityframework
    /// </summary>
    /// <typeparam name="T">type</typeparam>
    public class EfSpecificationEvaluator<T> : ISpecificationEvaluator<T> where T : class
    {
        /// <summary>
        /// Prepare custom query using specification
        /// </summary>
        /// <param name="build">starting query</param>
        /// <param name="specification">application specification</param>
        /// <param name="singleInstance">query for single instance ?</param>
        /// <returns>Custom query built using specification</returns>
        public IQueryable<T> GetQuery(IQueryable<T> build, ISpecification<T> specification, bool singleInstance)
        {
            var query = build;

            //Add where clause
            if (specification.Criteria != null)
            {
                query = query.Where(specification.Criteria);
            }

            //Add includes
            query = specification.Includes.Aggregate(query,
                (current, include) => current.Include(include));

            //return query if we are looking for single instance
            if (singleInstance)
            {
                return query;
            }

            //Apply orderby if needed
            if (specification.OrderBy != null)
            {
                query = query.OrderBy(specification.OrderBy);
            }
            else if (specification.OrderByDescending != null)
            {
                query = query.OrderByDescending(specification.OrderByDescending);
            }

            //Apply paging
            if (specification.IsPagingEnabled)
            {
                query = query.Skip(specification.Skip).Take(specification.Take);
            }

            return query;
        }
    }
}
