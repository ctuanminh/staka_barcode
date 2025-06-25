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
        public decimal BasePrice { get; set; }
		public int? NumOfProduct { get; set; }
        public ICollection<Inventory> Inventories { get; set; }
	}

	public class ProductApiResponse
    {
        public List<ProductDto> Data { get; set; }
        public int StatusCode { get; set; }
        public int Total { get; set; }
    }
    public class Inventory
    {
        public long ProductId { get; set; }              // Id của sản phẩm
        public string ProductCode { get; set; }          // Mã của sản phẩm
        public string ProductName { get; set; }          // Tên của sản phẩm

        public int BranchId { get; set; }                // Id của chi nhánh
        public string BranchName { get; set; }           // Tên của chi nhánh

        public double? OnHand { get; set; }              // Tồn kho theo chi nhánh
        public decimal? Cost { get; set; }               // Giá vốn sản phẩm (giá nhập)
        public double OnOrder { get; set; }              // Số lượng đặt từ nhà cung cấp
        public double Reserved { get; set; }             // Số lượng đã được đặt hàng

        public double MinQuality { get; set; }           // Định mức tồn tối thiểu
        public double MaxQuality { get; set; }           // Định mức tồn tối đa
    }
}
