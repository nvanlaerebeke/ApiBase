namespace ApiBase.Filter.Sorting
{
    public interface ISortFilter
    {
        string OrderBy { get; set; }
        SortDirection OrderDirection { get; set; }
    }
}