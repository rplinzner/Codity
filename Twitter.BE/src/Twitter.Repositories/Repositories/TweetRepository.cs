using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Twitter.Data.Context;
using Twitter.Data.Model;
using Twitter.Repositories.Helpers;
using Twitter.Repositories.Interfaces;

namespace Twitter.Repositories.Repositories
{
    public class TweetRepository : BaseRepository<Tweet>, ITweetRepository
    {
        public TweetRepository(TwitterDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<Tweet> GetByAsync(Expression<Func<Tweet, bool>> getBy, bool withTracking = false)
        {
            var query = CreateGetQuery(withTracking);

            return await query.FirstOrDefaultAsync(getBy);
        }

        public async Task<PagedList<Tweet>> GetPagedAsync(int pageNumber, int pageSize, bool withTracking = false)
        {
            var query = CreateGetQuery(withTracking)
                .OrderByDescending(c => c.CreationDate);

            return await PagedList<Tweet>.Create(query, pageNumber, pageSize);
        }

        private IQueryable<Tweet> CreateGetQuery(bool withTracking)
        {
            var query = _dbContext.Tweets
                .Include(c => c.Author)
                .Include(c => c.CodeSnippet)
                    .ThenInclude(c => c.ProgrammingLanguage)
                .Include(c => c.Likes)
                .Include(c => c.Comments) as IQueryable<Tweet>;

            if (!withTracking)
            {
                query = query.AsNoTracking();
            }

            return query;
        }
    }
}
