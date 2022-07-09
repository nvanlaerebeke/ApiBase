namespace ApiBase.Filter.Pagination
{
    public class PaginationFilter : IPaginationFilter
    {
        public int? PageNumber { get; set; }
        public int? PageSize { get; set; }
    }
}
