using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Twitter.Data.Context;
using Twitter.Data.Model;
using Twitter.Repositories.Helpers;
using Twitter.Repositories.Interfaces;

namespace Twitter.Repositories.Repositories
{
    public class UserRepository : BaseRepository<User>, IUserRepository
    {
        public UserRepository(TwitterDbContext dbContext) : base(dbContext)
        {
        }



        public async Task<PagedList<User>> SearchAsync(string query, int pageNumber, int pageSize, bool withTracking = false)
        {
            var result = await _dbContext.Users.FromSqlInterpolated(
                $"SELECT * FROM dbo.SearchUsers({query}, {pageNumber}, {pageSize})")
                .Include(c => c.Followers)
                .ToListAsync();

            var count = _dbContext.Users.FromSqlInterpolated(
                $"SELECT * FROM dbo.SearchUsersCount({query})")
                .Count();

            return new PagedList<User>(result, count, pageNumber, pageSize);
        }
    }
}
