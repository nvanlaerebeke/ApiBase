using System.Collections.Generic;

namespace ApiBase.Filter.Pagination
{
    public class PagedResult<T>
    {
        public PagedResult(IEnumerable<T> data, int totalRecords, PaginationFilter paginationFilter)
        {
            Data = data;
            TotalRecords = totalRecords;
            PaginationFilter = paginationFilter;
        }

        public IEnumerable<T> Data { get; }
        public int TotalRecords { get; }
        public PaginationFilter PaginationFilter { get; }
    }
}
