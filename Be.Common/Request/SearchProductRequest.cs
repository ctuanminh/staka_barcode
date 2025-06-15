
namespace Be.Common.Request
{
	public class SearchProductRequest : SearchRequest
	{
        public string Name { get; set; }
        public bool Status { get; set; }
        public Guid CategoryId { get; set; }
    }
}
