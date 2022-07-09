namespace ApiBase.Filter.Sorting
{
    public class SortQueryString
    {
        public SortQueryString() : this("", SortDirection.ASC)
        {
        }

        public SortQueryString(string orderBy, SortDirection sortDirection) : base()
        {
            OrderDirection = sortDirection;
            OrderBy = orderBy;
        }

        public SortDirection OrderDirection { get; set; }
        public string OrderBy { get; set; }
    }
}
