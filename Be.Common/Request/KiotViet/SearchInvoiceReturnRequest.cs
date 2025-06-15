namespace Be.Common.Request.KiotViet
{
    public class SearchInvoiceReturnRequest
    {
        /// <summary>
        /// Sắp xếp theo trường nào (ví dụ: "Name", "ReturnDate"...)
        /// </summary>
        public string? OrderBy { get; set; }

        /// <summary>
        /// Từ ngày trả hàng (yyyy-MM-dd)
        /// </summary>
        public DateTime? FromReturnDate { get; set; }

        /// <summary>
        /// Đến ngày trả hàng (yyyy-MM-dd)
        /// </summary>
        public DateTime? ToReturnDate { get; set; }

        /// <summary>
        /// Thời gian cập nhật sau thời điểm này
        /// </summary>
        public DateTime? LastModifiedFrom { get; set; }

        /// <summary>
        /// Số lượng bản ghi trong 1 trang (mặc định 20, tối đa 100)
        /// </summary>
        public int PageSize { get; set; } = 20;

        /// <summary>
        /// Vị trí bản ghi bắt đầu (ví dụ: 0, 20, 40...)
        /// </summary>
        public int CurrentItem { get; set; } = 0;

        /// <summary>
        /// Có bao gồm danh sách thanh toán hay không?
        /// </summary>
        public bool IncludePayment { get; set; } = false;

        /// <summary>
        /// Hướng sắp xếp: "asc" hoặc "desc"
        /// </summary>
        public string? OrderDirection { get; set; }
        public int[] BranchIds { get; set; }            // ID chi nhánh (optional)
    }
}
