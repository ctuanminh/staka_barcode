using System;

namespace FrmMain.Dto.Request
{
    public class SearchOrderRequest
    {
        public int[]? BranchIds { get; set; }                  // ID chi nhánh
        public long[]? CustomerIds { get; set; }               // ID khách hàng
        public string? CustomerCode { get; set; }              // Mã khách hàng
        public int[]? Status { get; set; }                     // Tình trạng đơn hàng
        public bool IncludePayment { get; set; }               // Lấy thông tin thanh toán
        public bool IncludeOrderDelivery { get; set; }         // Lấy thông tin giao hàng
        public DateTime? LastModifiedFrom { get; set; }        // Thời gian cập nhật từ
        public int? PageSize { get; set; }                     // Số item trong 1 trang (tối đa 100)
        public int CurrentItem { get; set; }                   // Item hiện tại
        public DateTime? ToDate { get; set; }                  // Thời gian cập nhật đến
        public string? OrderBy { get; set; }                   // Trường sắp xếp (ví dụ: orderBy=name)
        public string? OrderDirection { get; set; }            // Asc / Desc
        public DateTime? CreatedDate { get; set; }             // Thời gian tạo
        public int? SaleChannelId { get; set; }                // Id kênh bán hàng
    }
}
