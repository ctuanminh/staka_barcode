using Be.Common.Dtos.Category;

namespace Be.Common.Dtos.Product
{
	public class ProductDto
	{
        public int Id { get; set; }
        public string Code { get; set; }
        public string BarCode { get; set; }
        public string Name { get; set; }
        public string Unit { get; set; }    
        public string? Description { get; set; }
		public string? Content { get; set; }
		public float? Price { get; set; }
		public int? NumOfProduct { get; set; }
	}

	public class ProductApiResponse
    {
        public List<ProductDto> Data { get; set; }
        public int StatusCode { get; set; }
        public int Total { get; set; }
    }
}
