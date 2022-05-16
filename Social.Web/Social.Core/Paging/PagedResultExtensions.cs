using System;
using System.Linq;
using AutoMapper.QueryableExtensions;

namespace Social.Core.Paging
{
    public static class PagedResultExtensions
    {
        public static PagedResult<T> GetPaged<T>(this IQueryable<T> query, int page, int pageSize) where T : class
        {
            var result = new PagedResult<T>
            {
                CurrentPage = page,
                PageSize = pageSize,
                RowCount = query.Count()
            };

            var pageCount = (double)result.RowCount / pageSize;
            result.PageCount = (int)Math.Ceiling(pageCount);

            var skip = (page - 1) * pageSize;
            query = query.Skip(skip).Take(pageSize);


            result.Results = query.ToList();

            return result;
        }
        public static PagedResult<U> GetPaged<T, U>(this IQueryable<T> query,
            int page, int pageSize) where U : class
        {
            var result = new PagedResult<U>();
            result.CurrentPage = page;
            result.PageSize = pageSize;
            result.RowCount = query.Count();

            var pageCount = (double)result.RowCount / pageSize;
            result.PageCount = (int)Math.Ceiling(pageCount);

            var skip = (page - 1) * pageSize;
            result.Results = query.Skip(skip)
                .Take(pageSize)
                .ProjectTo<U>()
                .ToList();

            return result;
        }
    }
}
