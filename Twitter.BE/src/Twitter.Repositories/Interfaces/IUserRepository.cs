using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Twitter.Data.Model;
using Twitter.Repositories.Helpers;

namespace Twitter.Repositories.Interfaces
{
    public interface IUserRepository : IBaseRepository<User>
    {
        Task<PagedList<User>> SearchAsync(
           string query,
           int pageNumber,
           int pageSize,
           int exceptId,
           bool withTracking = false);

        Task<User> GetByAsync(
          Expression<Func<User, bool>> getBy,
          bool withTracking = false
        );
    }
}
