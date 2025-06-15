namespace Be.Common.Purchase_Order.Request
{
    public partial class SearchPurchaseOrderRequest
    {
        public int[]? BranchIds { get; set; } // ID chi nhánh, optional
        public int[]? Status { get; set; } // Tình trạng đặt hàng, optional
        public bool IncludePayment { get; set; } // Có lấy thông tin thanh toán
        public bool IncludeOrderDelivery { get; set; } // Có lấy thông tin giao hàng
        public DateTime? FromPurchaseDate { get; set; } // Từ ngày nhập hàng, optional
        public DateTime? ToPurchaseDate { get; set; } // Đến ngày nhập hàng, optional
        public int? PageSize { get; set; } // Số items trong 1 trang, mặc định 20, tối đa 200
        public int? CurrentItem { get; set; } // Số items trong 1 trang, mặc định 20, tối đa 200
        public string OrderBy { get; set; } // sắp xếp theo thuộc tính nào, mặc định là "purchaseDate"
        public string OrderDirection { get; set; } // Chiều sắp xếp, mặc định là "desc"
    }
}
