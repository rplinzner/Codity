using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Twitter.Data.Model;
using Twitter.Repositories.Helpers;

namespace Twitter.Repositories.Interfaces
{
    public interface ITweetRepository : IBaseRepository<Tweet>
    {
        Task<Tweet> GetByAsync(
            Expression<Func<Tweet, bool>> getBy,
            bool withTracking = false);

        Task<PagedList<Tweet>> GetPagedByAsync(
           Expression<Func<Tweet, bool>> getBy,
           int pageNumber,
           int pageSize,
           bool withTracking = false);
    }
}
