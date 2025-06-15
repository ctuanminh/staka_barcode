namespace Be.Common.Dtos.Invoice
{
    public class SearchInvoiceRequest
    {
        public int[] BranchIds { get; set; }            // ID chi nhánh (optional)
        public long[] CustomerIds { get; set; }         // ID khách hàng (optional)
        public string CustomerCode { get; set; }        // Mã khách hàng
        public int[] Status { get; set; }               // Tình trạng đặt hàng (optional)
        public bool IncludePayment { get; set; }        // Có lấy thông tin thanh toán
        public bool IncludeInvoiceDelivery { get; set; }// Hóa đơn có giao hàng hay không
        public DateTime? LastModifiedFrom { get; set; }  // Thời gian cập nhật (optional)
        public int PageSize { get; set; }              // Số items trong 1 trang, mặc định 20 items, tối đa 100 items
        public int CurrentItem { get; set; }             // Mục hiện tại (số trang)
        public DateTime? ToDate { get; set; }           // Thời gian cập nhật cho đến thời điểm toDate (optional)
        public string OrderBy { get; set; }             // Sắp xếp dữ liệu theo trường orderBy (ví dụ: "name")
        public string OrderDirection { get; set; }      // Sắp xếp kết quả theo: Tăng dần (Asc) hoặc giảm dần (Desc)
        public long? OrderId { get; set; }              // Lọc danh sách hóa đơn theo Id của đơn đặt hàng (optional)
        public DateTime? CreatedDate { get; set; }      // Thời gian tạo (optional)
        public DateTime? FromPurchaseDate { get; set; } // Từ ngày giao dịch (optional)
        public DateTime? ToPurchaseDate { get; set; } //Đến ngày giao dịch
    }
}
