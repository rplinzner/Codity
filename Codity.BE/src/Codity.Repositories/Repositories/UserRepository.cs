using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Codity.Data.Context;
using Codity.Data.Model;
using Codity.Repositories.Helpers;
using Codity.Repositories.Interfaces;

namespace Codity.Repositories.Repositories
{
    public class UserRepository : BaseRepository<User>, IUserRepository
    {
        public UserRepository(CodityDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<PagedList<User>> SearchAsync(string query, int pageNumber, int pageSize, int exceptId, bool withTracking = false)
        {
            var constructedQuery = new StringBuilder();
            var splittedQuery = query.Split(' ');

            constructedQuery.Append($"\"{splittedQuery[0]}*\"");
            if (splittedQuery.Length > 1)
            {
                for (int i = 1; i < splittedQuery.Length; i++)
                {
                    constructedQuery.Append($" OR \"{splittedQuery[i]}*\"");
                }
            }

            var result = await _dbContext.Users.FromSqlInterpolated(
                $"SELECT * FROM dbo.SearchUsers({constructedQuery.ToString()}, {pageNumber}, {pageSize}, {exceptId})")
                .ToListAsync();

            if (result.Any())
            {
                var userIds = result.Select(c => c.Id);
                await _dbContext.Follows.Where(c => userIds.Contains(c.FollowingId)).LoadAsync();
            }

            var count = _dbContext.Users.FromSqlInterpolated(
                $"SELECT * FROM dbo.SearchUsersCount({constructedQuery.ToString()}, {exceptId})")
                .Count();

            return new PagedList<User>(result, count, pageNumber, pageSize);
        }

        public virtual async Task<User> GetByAsync(
          Expression<Func<User, bool>> getBy,
          bool withTracking = false
        )
        {
            var query = _dbContext.Users
                .Include(c => c.Followers)
                .Include(c => c.Following)
                .Include(c => c.Gender)
                .Include(c => c.Posts).ThenInclude(c => c.Likes)
                .Include(c => c.Posts).ThenInclude(c => c.Comments)
                .Include(c => c.Posts).ThenInclude(c => c.CodeSnippet)
                        .ThenInclude(c => c.ProgrammingLanguage)
                as IQueryable<User>;

            if (!withTracking)
            {
                query = query.AsNoTracking();
            }

            return await query.FirstOrDefaultAsync(getBy);
        }

    }
}
