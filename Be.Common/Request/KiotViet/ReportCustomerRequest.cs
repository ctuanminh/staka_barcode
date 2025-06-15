namespace Be.Common.Request.KiotViet
{
    public partial class ReportCustomerRequest
    {
        public int compareMonth { get; set; }
        public int referenceMonth { get; set; }        
        public int[] BranchIds { get; set; }            // ID chi nhánh (optional)
        public long[] CustomerIds { get; set; }         // ID khách hàng (optional)
        public string CustomerCode { get; set; }        // Mã khách hàng
        public int[] Status { get; set; }               // Tình trạng đặt hàng (optional)
        public bool IncludeCustomerGroup { get; set; } = true;  // Lấy thông tin nhóm khách hàng        
        public bool includeRemoveIds { get; set; } = false;  // Lấy thông tin nhóm khách hàng        
        public DateTime? LastModifiedFrom { get; set; }  // Thời gian cập nhật (optional)
        public int PageSize { get; set; }              // Số items trong 1 trang, mặc định 20 items, tối đa 100 items
        public int CurrentItem { get; set; }             // Mục hiện tại (số trang)
        public DateTime? ToDate { get; set; }           // Thời gian cập nhật cho đến thời điểm toDate (optional)
        public string OrderBy { get; set; } = "createdDate";            // Sắp xếp dữ liệu theo trường orderBy (ví dụ: "name")
        public string OrderDirection { get; set; } = "Desc";      // Sắp xếp kết quả theo: Tăng dần (Asc) hoặc giảm dần (Desc)        
        public DateTime? CreatedDate { get; set; }      // Thời gian tạo (optional)
        public int GroupId { get; set; } // lọc theo nhóm khách hàng
    }    
}
