using Be.Core.BaseEntities;

namespace Be.Core.Entities
{
    public class SupplierEntity : AuditedEntity
    {
        public long KiotId { get; set; }                 // ID nhà cung cấp trên KiotViet
        public string Code { get; set; }             // Mã nhà cung cấp
        public string Name { get; set; }             // Tên nhà cung cấp
        public string ContactNumber { get; set; }    // Điện thoại
        public string Email { get; set; }            // Email
        public string Address { get; set; }          // Địa chỉ
        public string LocationName { get; set; }     // Khu vực
        public string WardName { get; set; }         // Phường xã
        public string Organization { get; set; }     // Tên công ty
        public string TaxCode { get; set; }          // Mã số thuế
        public string Comments { get; set; }         // Ghi chú
        public string Groups { get; set; }           // Danh sách nhóm nhà cung cấp ngăn cách bởi dấu phẩy
        public bool IsActive { get; set; }           // Trạng thái hoạt động
        public DateTime ModifiedDate { get; set; }   // Thời gian cập nhật gần nhất
        public DateTime CreatedDate { get; set; }    // Thời gian tạo
        public long RetailerId { get; set; }         // ID gian hàng
        public long BranchId { get; set; }           // ID chi nhánh
        public string CreatedBy { get; set; }        // Người tạo
        public decimal Debt { get; set; }            // Nợ cần trả
        public decimal TotalInvoiced { get; set; }   // Tổng mua
        public decimal TotalInvoicedWithoutReturn { get; set; } // Tổng mua trừ trả hàng
    }
}
