using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Twitter.Repositories.Helpers
{
    public class PagedList<T> : List<T>
    {
        public int PageSize { get; private set; }
        public int CurrentPage { get; private set; }
        public int TotalPages { get; private set; }
        public int TotalCount { get; private set; }

        public PagedList(List<T> items, int count, int pageNumber, int pageSize)
        {
            TotalCount = count;
            CurrentPage = pageNumber;
            TotalPages = (int)Math.Ceiling(count / (double)pageSize);
            PageSize = pageSize;
            AddRange(items);
        }

        public static async Task<PagedList<T>> Create(IQueryable<T> source, int pageNumber, int pageSize)
        {
            var items = await source.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();
            var count = source.Count();

            return new PagedList<T>(items, count, pageNumber, pageSize);
        }
    }
}
