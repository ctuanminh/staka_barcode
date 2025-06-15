
namespace Be.Common.Dtos.Product
{
	public class ProductUpdateRequest
	{
		public Guid Id { get; set; }
		public string Description { get; set; }
		public string Content { get; set; }
		public float Price { get; set; }
		public Guid CategoryId { get; set; }
	}
}
