using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Twitter.Data.Context;
using Twitter.Data.Model;
using Twitter.Repositories.Helpers;
using Twitter.Repositories.Interfaces;

namespace Twitter.Repositories.Repositories
{
    public class BaseRepository<T> : IBaseRepository<T> where T : class, IBaseEntity
    {
        protected readonly TwitterDbContext _dbContext;

        public BaseRepository(TwitterDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public virtual async Task AddAsync(T entity)
        {
            _dbContext.Set<T>().Add(entity);
            await _dbContext.SaveChangesAsync();
        }

        public virtual async Task AddRangeAsync(IEnumerable<T> entities)
        {
            _dbContext.Set<T>().AddRange(entities);
            await _dbContext.SaveChangesAsync();
        }

        public virtual async Task<bool> ExistAsync(
            Expression<Func<T, bool>> getBy,
            params Expression<Func<T, object>>[] includes)
        {
            var query = _dbContext.Set<T>() as IQueryable<T>;

            foreach (var include in includes)
            {
                query = query.Include(include);
            }

            return await query.AnyAsync(getBy);
        }

        public virtual async Task<IEnumerable<T>> GetAllAsync(
            bool withTracking = false,
            params Expression<Func<T, object>>[] includes)
        {
            var query = _dbContext.Set<T>() as IQueryable<T>;

            if (!withTracking)
            {
                query = query.AsNoTracking();
            }

            foreach (var include in includes)
            {
                query = query.Include(include);
            }

            return await query.ToListAsync();
        }

        public virtual async Task<IEnumerable<T>> GetAllByAsync(
            Expression<Func<T, bool>> getBy,
            bool withTracking = false,
            params Expression<Func<T, object>>[] includes)
        {
            return await GetAllByQuery(getBy, withTracking, includes)
                .ToListAsync();
        }

        public virtual async Task<PagedList<T>> GetAllByAsync(
           Expression<Func<T, bool>> getBy,
           int pageNumber,
           int pageSize,
           bool withTracking = false,
           params Expression<Func<T, object>>[] includes)
        {
            var query = GetAllByQuery(getBy, withTracking, includes);
            
            return await PagedList<T>.Create(query, pageNumber, pageSize);
        }

        public virtual async Task<T> GetAsync(
            int id,
            bool withTracking = false,
            params Expression<Func<T, object>>[] includes)
        {
            var query = _dbContext.Set<T>() as IQueryable<T>;

            if (!withTracking)
            {
                query = query.AsNoTracking();
            }

            foreach (var include in includes)
            {
                query = query.Include(include);
            }

            return await query.FirstOrDefaultAsync(c => c.Id == id);
        }

        public virtual async Task<T> GetByAsync(
            Expression<Func<T, bool>> getBy,
            bool withTracking = false,
            params Expression<Func<T, object>>[] includes)
        {
            var query = _dbContext.Set<T>() as IQueryable<T>;

            if (!withTracking)
            {
                query = query.AsNoTracking();
            }

            foreach (var include in includes)
            {
                query = query.Include(include);
            }

            return await query.FirstOrDefaultAsync(getBy);
        }

        public virtual async Task RemoveAsync(T entity)
        {
            _dbContext.Set<T>().Remove(entity);
            await _dbContext.SaveChangesAsync();
        }

        public virtual async Task RemoveRangeAsync(IEnumerable<T> entities)
        {
            _dbContext.Set<T>().RemoveRange(entities);
            await _dbContext.SaveChangesAsync();
        }

        public virtual async Task UpdateAsync(T entity)
        {
            _dbContext.Set<T>().Update(entity);
            await _dbContext.SaveChangesAsync();
        }

        public virtual async Task UpdateRangeAsync(IEnumerable<T> entities)
        {
            _dbContext.Set<T>().UpdateRange(entities);
            await _dbContext.SaveChangesAsync();
        }

        private IQueryable<T> GetAllByQuery(
            Expression<Func<T, bool>> getBy,
            bool withTracking = false,
            params Expression<Func<T, object>>[] includes)
        {
            var query = _dbContext.Set<T>() as IQueryable<T>;

            if (!withTracking)
            {
                query = query.AsNoTracking();
            }

            foreach (var include in includes)
            {
                query = query.Include(include);
            }

            return query.Where(getBy);
        }
    }
}
