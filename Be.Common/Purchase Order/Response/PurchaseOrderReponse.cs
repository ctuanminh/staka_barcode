namespace Be.Common.Purchase_Order.Response
{
    public partial class PurchaseOrderResponse
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
        public decimal Total { get; set; } // Tổng tiền hàng
        public decimal TotalPayment { get; set; } // Tiền đã trả NCC
        public int Status { get; set; }
        public string StatusValue
        {
            get
            {
                return Status switch
                {
                    1 => "Phiếu tạm",
                    3 => "Đã nhập hàng",
                    _ => "Đã huỷ"
                };
            }
        }

        public DateTime CreatedDate { get; set; }
        public long SupplierId { get; set; }
        public string SupplierName { get; set; }
        public string SupplierCode { get; set; }
        public long PurchaseById { get; set; }
        public string PurchaseName { get; set; }
        public decimal ExReturnSuppliers { get; set; }
        public decimal ExReturnThirdParty { get; set; }
        public List<PurchaseOrderDetail> PurchaseOrderDetails { get; set; }
        public List<Payment>? Payments { get; set; }
        public int Quantity => PurchaseOrderDetails.Count();
    }

    public partial class PurchaseOrderDetail
    {
        public long ProductId { get; set; }
        public string ProductCode { get; set; }
        public string ProductBarCode { get; set; }
        public string ProductName { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public string Unit { get; set; }
        public decimal Discount { get; set; }
        public double DiscountRatio { get; set; }
        public bool Checked { get; set; }
        private decimal _total;
        public decimal Total
        {
            get => (Price * Quantity) - Discount;
            set => _total = value;
        }
    }

    public partial class PurchaseOrderPagedData
    {
        public List<PurchaseOrderResponse> Data { get; set; }
        public int Total { get; set; }
        public int PageSize { get; set; }
        public int CurrentItem { get; set; }
    }
    public class Payment
    {
        public long Id { get; set; }           // Id thanh toán
        public string Code { get; set; }       // Mã thanh toán
        public string Method { get; set; }     // Phương thức thanh toán
        public int Status { get; set; }        // Trạng thái
        public string StatusValue { get; set; } // Tên trạng thái
        public DateTime TransDate { get; set; } // Ngày thanh toán
    }

}
