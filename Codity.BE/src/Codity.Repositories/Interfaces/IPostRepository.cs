using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Codity.Data.Model;
using Codity.Repositories.Helpers;

namespace Codity.Repositories.Interfaces
{
    public interface IPostRepository : IBaseRepository<Post>
    {
        Task<Post> GetByAsync(
            Expression<Func<Post, bool>> getBy,
            bool withTracking = false);

        Task<PagedList<Post>> GetPagedByAsync(
           Expression<Func<Post, bool>> getBy,
           int pageNumber,
           int pageSize,
           bool withTracking = false);
    }
}
