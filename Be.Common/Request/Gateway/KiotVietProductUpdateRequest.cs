namespace Be.Common.Request.Gateway
{
    public class KiotVietProductUpdateRequest
    {
        public string Id { get; set; }
        public int Attempt { get; set; }
        public List<ProductNotification> Notifications { get; set; }
    }

    public class ProductNotification
    {
        public string Action { get; set; }
        public List<ProductData> Data { get; set; }
    }

    public class ProductData
    {
        public long Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string FullName { get; set; }
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public long? MasterProductId { get; set; }
        public bool AllowsSale { get; set; }
        public bool HasVariants { get; set; }
        public decimal BasePrice { get; set; }
        public double? Weight { get; set; }
        public string Unit { get; set; }
        public long? MasterUnitId { get; set; }
        public double? ConversionValue { get; set; }
        public DateTime? ModifiedDate { get; set; }

        public List<ProductAttribute> Attributes { get; set; }
        public List<ProductUnit> Units { get; set; }
        public List<ProductInventory> Inventories { get; set; }
        public List<ProductPriceBook> PriceBooks { get; set; }
        public List<ProductImage> Images { get; set; }
    }

    public class ProductAttribute
    {
        public long ProductId { get; set; }
        public string AttributeName { get; set; }
        public string AttributeValue { get; set; }
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

    public class ProductInventory
    {
        public long ProductId { get; set; }
        public string ProductCode { get; set; }
        public string ProductName { get; set; }
        public int BranchId { get; set; }
        public string BranchName { get; set; }
        public decimal Cost { get; set; }
        public double OnHand { get; set; }
        public double Reserved { get; set; }
    }

    public class ProductPriceBook
    {
        public long ProductId { get; set; }
        public long PriceBookId { get; set; }
        public string PriceBookName { get; set; }
        public decimal Price { get; set; }
        public bool IsActive { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }

    public class ProductImage
    {
        public string Image { get; set; }
    }
}
