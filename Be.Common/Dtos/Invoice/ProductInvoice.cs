using System.Text.Json.Serialization;

namespace Be.Common.Dtos.Invoice
{
    public class ProductInvoice
    {
        [JsonPropertyName("id")]
        public long? id { get; set; }
        
        [JsonPropertyName("code")]
        public string? code { get; set; }        
        [JsonPropertyName("unit")]
        public string? unit { get; set; }        
    }

    public class ProductUnit
    {
        public long Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string FullName { get; set; }
        public string Unit { get; set; }
        public double ConversionValue { get; set; }
        public decimal BasePrice { get; set; }
    }

    public class ProductPriceBook
    {
        public long PriceBookId { get; set; }
        public string PriceBookName { get; set; }
        public long ProductId { get; set; }
        public bool IsActive { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public decimal Price { get; set; }
    }

    public class ProductFormula
    {
        public long MaterialId { get; set; }
        public string MaterialCode { get; set; }
        public string MaterialFullName { get; set; }
        public string MaterialName { get; set; }
        public int Quantity { get; set; }
        public decimal BasePrice { get; set; }
        public long? ProductId { get; set; }        
    }

    public class ProductSerial
    {
        public long ProductId { get; set; }
        public string SerialNumber { get; set; }
        public int Status { get; set; }
        public int BranchId { get; set; }
        public double? Quantity { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
    }

    public class ProductBatchExpire
    {
        public long ProductId { get; set; }
        public double OnHand { get; set; }
        public string BatchName { get; set; }
        public DateTime ExpireDate { get; set; }
        public string FullNameVirgule { get; set; }
        public int BranchId { get; set; }
    }

    public enum ProductStatus
    {
        Active,
        Inactive
    }

    public class ProductInventory
    {
        public int BranchId { get; set; }          // ID chi nhánh
        public string BranchName { get; set; }     // Tên chi nhánh
        public double? OnHand { get; set; }            // Tồn kho thực tế
        public double? Reserved { get; set; }          // Số lượng đã đặt
        public long? Available { get; set; }         // Số lượng có thể bán
    }
    public class ProductAttribute
    {
        public long ProductId { get; set; }         // ID thuộc tính
        public string? AttributeName { get; set; }   // Tên thuộc tính
        public string? AttributeValue { get; set; }  // Giá trị thuộc tính
    }

    public class ProductShelf
    {
        public int BranchId { get; set; }
        public string BranchName { get; set; }
        public string ProductShelves { get; set; }
    }

    public class Inventory
    {
        public int ProductId { get; set; }
        public int BranchId { get; set; }
        public string BranchName { get; set; }
        public decimal Cost { get; set; }
        public int OnHand { get; set; }
        public int Reserved { get; set; }
        public int ActualReserved { get; set; }
        public int MinQuantity { get; set; }
        public int MaxQuantity { get; set; }
        public bool IsActive { get; set; }
        public int OnOrder { get; set; }
    }

    public class PriceBook
    {
        public int ProductId { get; set; }
        public int PriceBookId { get; set; }
        public string PriceBookName { get; set; }
        public decimal Price { get; set; }
        public bool IsActive { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}
