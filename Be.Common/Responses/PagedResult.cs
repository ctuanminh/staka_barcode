namespace Be.Common.Responses
{
    public class PagedResult<T>
    {
        public List<T> Items { get; set; }
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public int TotalCount { get; set; }
        public int TotalPage => (int)Math.Ceiling(TotalCount / (double)PageSize);

        public PagedResult<TDto> ChangeType<TDto>(Func<T, TDto> cast) => new PagedResult<TDto>()
        {
            Items = Items.Select(cast).ToList(),
            PageIndex = PageIndex,
            PageSize = PageSize,
            TotalCount = TotalCount
        };
    }
}
