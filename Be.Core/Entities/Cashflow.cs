using Be.Core.BaseEntities;

namespace Be.Core.Entities
{
    public class Cashflow : AuditedEntity
    {
        public string Code { get; set; } = string.Empty;        // Mã dòng tiền
        public int BranchId { get; set; }    // ID chi nhánh
        public string? Address { get; set; }    // Địa chỉ
        public string? WardName { get; set; }   // Phường/Xã
        public string? ContactNumber { get; set; } // Số điện thoại
        public int UsedForFinancialReporting { get; set; } // Dùng cho báo cáo tài chính
        public int? CashFlowGroupId { get; set; } // ID nhóm dòng tiền
        public string Method { get; set; } = string.Empty;      // Phương thức thanh toán
        public string PartnerType { get; set; } = string.Empty; // Loại đối tác
        public int? PartnerId { get; set; }  // ID đối tác
        public int Status { get; set; }      // Mã trạng thái
        public string StatusValue { get; set; } = string.Empty; // Tên trạng thái
        public string TransDate { get; set; } = string.Empty;   // Ngày giao dịch
        public decimal Amount { get; set; }      // Số tiền
        public string PartnerName { get; set; } = string.Empty; // Tên đối tác
        public string User { get; set; } = string.Empty;        // Người dùng
        public int? AccountId { get; set; }  // ID tài khoản
        public string? Description { get; set; } // Ghi chú
    }
}
