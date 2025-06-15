namespace Be.Common.Request
{
    public class CashFlowRequest
    {
        public int[]? BranchIds { get; set; }           // Lọc theo chi nhánh
        public string[]? Code { get; set; }             // Lọc theo mã
        public int? UserId { get; set; }                // Lọc theo người dùng
        public int? AccountId { get; set; }             // Lọc theo tài khoản
        public string? PartnerType { get; set; }        // Lọc theo loại đối tác
        public string[]? Method { get; set; }           // Lọc theo phương thức
        public int[]? CashFlowGroupId { get; set; }     // Lọc theo nhóm
        public int? UsedForFinancialReporting { get; set; } // Dùng cho báo cáo
        public string? PartnerName { get; set; }        // Tìm theo tên đối tác
        public string? ContactNumber { get; set; }      // Tìm theo SĐT
        public bool? IsReceipt { get; set; }            // Là phiếu thu
        public bool? IncludeAccount { get; set; }       // Bao gồm thông tin tài khoản
        public bool? IncludeBranch { get; set; }        // Bao gồm thông tin chi nhánh
        public bool? IncludeUser { get; set; }          // Bao gồm thông tin người dùng
        public string? StartDate { get; set; }          // Từ ngày
        public string? EndDate { get; set; }            // Đến ngày
        public int? Status { get; set; }                // Lọc theo trạng thái
        public int? PageSize { get; set; }              // Số lượng trên trang
        public int? CurrentItem { get; set; }           // Vị trí bắt đầu
    }
}
