namespace Be.Common.Dtos.CashFlow
{
    public class CashFlowReponse
    {
        public long Id { get; set; }                        // Id phiếu
        public string Code { get; set; } = string.Empty;    // Mã phiếu
        public string Address { get; set; } = string.Empty; // Địa chỉ
        public int BranchId { get; set; }                   // Id chi nhánh
        public string WardName { get; set; } = string.Empty; // Tên phường
        public string ContactNumber { get; set; } = string.Empty; // Số điện thoại
        public long CreatedBy { get; set; }                 // Id người tạo
        public int UsedForFinancialReporting { get; set; }  // Dùng cho báo cáo tài chính
        public int? CashFlowGroupId { get; set; }           // Id loại thu chi
        public string Method { get; set; } = string.Empty;  // Phương thức thanh toán
        public string PartnerType { get; set; } = string.Empty; // Người nộp/nhận
        public long? PartnerId { get; set; }                // Id người nộp/nhận
        public int Status { get; set; }                     // Trạng thái phiếu
        public string StatusValue { get; set; } = string.Empty; // Trạng thái phiếu bằng chữ
        public DateTime TransDate { get; set; }             // Ngày tạo
        public decimal Amount { get; set; }                 // Giá trị
        public string PartnerName { get; set; } = string.Empty; // Tên người nộp/nhận
        public string User { get; set; } = string.Empty;    // Tên người tạo
        public int? AccountId { get; set; }                 // Id tài khoản ngân hàng
        public string Description { get; set; } = string.Empty; // Ghi chú
    }

    public class CashFlowApiReponse
    {
        public List<CashFlowReponse> Data { get; set; }
        public int Total { get; set; }
        public int PageSize { get; set; }
        public int CurrentItem { get; set; }
    }
}
