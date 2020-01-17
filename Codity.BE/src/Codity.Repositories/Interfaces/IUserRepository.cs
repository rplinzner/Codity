using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Codity.Data.Model;
using Codity.Repositories.Helpers;

namespace Codity.Repositories.Interfaces
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
