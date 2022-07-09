using System.Collections.Generic;
using System.Linq;
using ApiBase.Filter.Pagination;
using ApiBase.Object;

namespace ApiBase.Controller.Response
{
    public class PageResponse<T> where T : IAPIObject
    {
        public PageResponse()
        {
        }

        public PageResponse(IEnumerable<T> data, int? total = null, PaginationFilter paginationFilter = null)
        {
            Data = data;
            TotalRecords = total;
            if (paginationFilter != null)
            {
                Page = (paginationFilter.PageSize == null || paginationFilter.PageSize == -1) ? null : paginationFilter.PageNumber;
                Size = (paginationFilter.PageSize == null || paginationFilter.PageSize == -1) ? null : paginationFilter.PageSize;
                if (Page != null && Size != null)
                {
                    if (!data.Any() || data.Count() < paginationFilter.PageSize || Page == null || TotalRecords < (Page * Size))
                    {
                        NextPage = null;
                    }
                    else
                    {
                        NextPage = paginationFilter.PageNumber + 1;
                    }
                    PreviousPage = (paginationFilter.PageNumber > 0) ? (paginationFilter.PageNumber - 1) : 0;
                }
            }
        }

        public IEnumerable<T> Data { get; set; }
        public int? Page { get; set; } = null;
        public int? Size { get; set; } = null;
        public int? NextPage { get; set; } = null;
        public int? PreviousPage { get; set; } = null;
        public int? TotalRecords { get; set; } = null;
    }
}
