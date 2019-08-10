using ApplicationCore.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Infrastructure.Data.EntityFramework.Repository
{
    /// <summary>
    /// Entity framework repository
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class EfRepository<T> : IAsyncRepository<T> where T : class
    {
        protected readonly ImageGalleryDbContext dbContext;
        protected readonly ISpecificationEvaluator<T> evaluator;

        public EfRepository(ImageGalleryDbContext dbContext, ISpecificationEvaluator<T> evaluator)
        {
            this.dbContext = dbContext;
            this.evaluator = evaluator;
        }

        /// <summary>
        /// Add instance to database
        /// </summary>
        /// <param name="entity">new instance</param>
        /// <returns>error message if error happaned, otherwise return instance</returns>
        public virtual async Task<(T instance, string errorMessage)> AddAsync(T entity)
        {
            try
            {
                dbContext.Set<T>().Add(entity);
                await dbContext.SaveChangesAsync();
                return (entity, string.Empty);
            }
            catch(DbUpdateException e) { return (entity, e?.InnerException?.Message ?? e.Message); }
        }

        /// <summary>
        /// Count number of specific instances in database
        /// </summary>
        /// <returns>number of instances in database</returns>
        public virtual async Task<int> CountAsync()
        {
            try
            {
                return await dbContext.Set<T>().CountAsync();
            }
            catch { return 0; }
        }

        /// <summary>
        /// Delete instance
        /// </summary>
        /// <param name="entity">The instance we want to delete</param>
        /// <returns></returns>
        public virtual async Task<bool> DeleteAsync(T entity)
        {
            try
            {
                dbContext.Set<T>().Remove(entity);
                await dbContext.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Get instance by id
        /// </summary>
        /// <param name="id">id of instance</param>
        /// <returns>instance</returns>
        public virtual async Task<T> GetByIdAsync(object id)
        {
            try
            {
                return await dbContext.Set<T>().FindAsync(id);
            }
            catch { return null; } 
        }

        /// <summary>
        /// Get all instances
        /// </summary>
        /// <returns>List of instances</returns>
        public virtual async Task<List<T>> ListAllAsync()
        {
            try
            {
                return await dbContext.Set<T>().AsNoTracking().ToListAsync();
            }
            catch { return null; }
        }

        /// <summary>
        /// Update instance
        /// </summary>
        /// <param name="entity">Instance we want do update</param>
        /// <returns></returns>
        public virtual async Task<bool> UpdateAsync(T entity)
        {
            try
            {
                dbContext.Entry(entity).State = EntityState.Modified;
                await dbContext.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Get Instances using application specification
        /// </summary>
        /// <param name="spec">specification</param>
        /// <returns></returns>
        public virtual async Task<List<T>> ListAsync(ISpecification<T> spec)
        {
            try
            {
                return await ApplySpecification(spec).AsNoTracking().ToListAsync();
            }
            catch { return null; }
        }


        /// <summary>
        /// Count number of specific instances in database for specification
        /// </summary>
        /// <param name="spec">specification</param>
        /// <returns></returns>
        public virtual async Task<int> CountAsync(ISpecification<T> spec)
        {
            try
            {
                return await ApplySpecification(spec).CountAsync();
            }
            catch { return 0; }
        }

        /// <summary>
        /// Pagination
        /// </summary>
        /// <param name="skip">number of instances to skip</param>
        /// <param name="take">number of instances to take</param>
        /// <returns>List of instances</returns>
        public virtual async Task<List<T>> ListPaginationAsync(int skip, int take)
        {
            try
            {
                return await dbContext.Set<T>().Skip(skip).Take(take).AsNoTracking().ToListAsync();
            }
            catch { return null; }
        }

        /// <summary>
        /// Get single instance using specification
        /// </summary>
        /// <param name="spec">specification</param>
        /// <returns>requested instance</returns>
        public virtual async Task<T> GetSingleInstanceAsync(ISpecification<T> spec)
        {
            try
            {
                var result = await ApplySpecification(spec, true).FirstOrDefaultAsync();
                return result;
            }
            catch { return null; }
        }

        /// <summary>
        /// Apply specification using entityframework specification evaluator
        /// </summary>
        /// <param name="spec">specification</param>
        /// <param name="singleInstance">single instance query ?</param>
        /// <returns>query</returns>
        private IQueryable<T> ApplySpecification(ISpecification<T> spec, bool singleInstance = false)
        {
            return evaluator.GetQuery(dbContext.Set<T>().AsQueryable(), spec, singleInstance);
        }
    }
}
