namespace Be.Common.Purchase_Order.Response
{
    public partial class PurchaseOrderReponse
    {
        public long Id { get; set; }
        public long RetailerId { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
        public long BranchId { get; set; }
        public string BranchName { get; set; }
        public DateTime PurchaseDate { get; set; }
        public decimal Discount { get; set; }
        public decimal DiscountRatio { get; set; }
        public decimal Total { get; set; }
        public decimal TotalPayment { get; set; }
        public int Status { get; set; }
        public DateTime CreatedDate { get; set; }
        public long SupplierId { get; set; }
        public string SupplierName { get; set; }
        public string SupplierCode { get; set; }
        public long PurchaseById { get; set; }
        public string PurchaseName { get; set; }
        public decimal ExReturnSuppliers { get; set; }
        public decimal ExReturnThirdParty { get; set; }
        public List<PurchaseOrderDetail> PurchaseOrderDetails { get; set; }
    }

    public partial class PurchaseOrderDetail
    {
        public long ProductId { get; set; }
        public string ProductCode { get; set; }
        public string ProductName { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public decimal Discount { get; set; }
    }

    public partial class PurchaseOrderPagedData
    {
        public List<PurchaseOrderReponse> Data { get; set; }
        public int Total { get; set; }
        public int PageSize { get; set; }
        public int CurrentItem { get; set; }
    }

}
