namespace ApiBase.Filter.Pagination
{
    public class PaginationQueryString
    {
        public PaginationQueryString()
        {
            PageNumber = 1;
            PageSize = -1;
        }

        public PaginationQueryString(int pageNumber, int pageSize)
        {
            PageNumber = pageNumber;
            PageSize = pageSize;
        }

        public int? PageNumber { get; set; }
        public int? PageSize { get; set; }
    }
}
