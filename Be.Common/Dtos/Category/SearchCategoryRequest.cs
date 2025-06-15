using Be.Common.Request;

namespace Be.Common.Dtos.Category
{
	public class SearchCategoryRequest : SearchRequest
	{
        public string Name { get; set; }
    }
}
