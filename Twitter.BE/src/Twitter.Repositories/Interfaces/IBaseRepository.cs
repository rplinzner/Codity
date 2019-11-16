using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Twitter.Data.Model;
using Twitter.Repositories.Helpers;

namespace Twitter.Repositories.Interfaces
{
    public interface IBaseRepository<T> where T : class, IBaseEntity
    {
        Task AddAsync(T entity);
        Task AddRangeAsync(IEnumerable<T> entities);
        Task UpdateAsync(T entity);
        Task UpdateRangeAsync(IEnumerable<T> entities);
        Task RemoveAsync(T entity);
        Task RemoveRangeAsync(IEnumerable<T> entities);

        Task<T> GetAsync(
            int id,
            bool withTracking = false,
            params Expression<Func<T, object>>[] includes);

        Task<T> GetByAsync(
            Expression<Func<T, bool>> getBy,
            bool withTracking = false,
            params Expression<Func<T, object>>[] includes);

        Task<IEnumerable<T>> GetAllAsync(
            bool withTracking = false,
            params Expression<Func<T, object>>[] includes);

        Task<PagedList<T>> GetAllByAsync(
           Expression<Func<T, bool>> getBy,
           int pageNumber,
           int pageSize,
           bool withTracking = false,
           params Expression<Func<T, object>>[] includes);

        Task<IEnumerable<T>> GetAllByAsync(
            Expression<Func<T, bool>> getBy,
            bool withTracking = false,
            params Expression<Func<T, object>>[] includes);

        Task<bool> ExistAsync(
            Expression<Func<T, bool>> getBy,
            params Expression<Func<T, object>>[] includes);
    }
}
