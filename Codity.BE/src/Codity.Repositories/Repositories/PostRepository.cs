using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Codity.Data.Context;
using Codity.Data.Model;
using Codity.Repositories.Helpers;
using Codity.Repositories.Interfaces;

namespace Codity.Repositories.Repositories
{
    public class PostRepository : BaseRepository<Post>, IPostRepository
    {
        public PostRepository(CodityDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<Post> GetByAsync(Expression<Func<Post, bool>> getBy, bool withTracking = false)
        {
            var query = CreateGetQuery(withTracking);

            return await query.FirstOrDefaultAsync(getBy);
        }

        public async Task<PagedList<Post>> GetPagedByAsync(Expression<Func<Post, bool>> getBy, int pageNumber, int pageSize, bool withTracking = false)
        {
            var query = CreateGetQuery(withTracking)
                .OrderByDescending(c => c.CreationDate)
                .Where(getBy);

            return await PagedList<Post>.Create(query, pageNumber, pageSize);
        }

        private IQueryable<Post> CreateGetQuery(bool withTracking)
        {
            var query = _dbContext.Posts
                .Include(c => c.Author)
                .Include(c => c.CodeSnippet)
                    .ThenInclude(c => c.ProgrammingLanguage)
                .Include(c => c.Likes)
                .Include(c => c.Comments) as IQueryable<Post>;

            if (!withTracking)
            {
                query = query.AsNoTracking();
            }

            return query;
        }
    }
}
