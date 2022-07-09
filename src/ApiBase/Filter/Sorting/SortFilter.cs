namespace ApiBase.Filter.Sorting
{
    public class SortFilter : ISortFilter
    {
        public SortDirection OrderDirection { get; set; }
        public string OrderBy { get; set; }
    }
}
