namespace Be.Common.Request
{
    public class SearchRequest
    {
        public string SortBy { get; set; }
        public string FilterBy { get; set; }
        public int PageSize { get; set; }
        public int PageIndex { get; set; }

    }
}
