using Be.Core.BaseEntities;

namespace Be.Core.Entities
{
	public class Product : AuditedEntity
	{        
        public string Code { get; set; }                // Mã sản phẩm
        public string BarCode { get; set; }
        public string Name { get; set; }                // Tên sản phẩm
        public int CategoryId { get; set; }             // ID danh mục
        public decimal BasePrice { get; set; }          // Giá gốc
        public decimal RetailPrice { get; set; }        // Giá bán lẻ
        public double? Weight { get; set; }             // Khối lượng
        public string? Unit { get; set; }               // Đơn vị tính
        public bool? AllowsSale { get; set; }            // Cho phép bán
        public bool IsActive { get; set; }
        public string? Description { get; set; }        // Mô tả
        public string? Attributes { get; set; }  // Thuộc tính
        public string? Inventories { get; set; } // Thông tin tồn kho
        public Category Category { get; set; }
    }
    
}
