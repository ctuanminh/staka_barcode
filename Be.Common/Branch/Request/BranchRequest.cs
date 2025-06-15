namespace Be.Common.Branch.Request
{
    public partial class BranchRequest
    {
        /// <summary>
        /// Thời gian cập nhật (ISO 8601 format). Null nếu không lọc theo thời gian.
        /// </summary>
        public DateTime? LastModifiedFrom { get; set; }

        /// <summary>
        /// Số items trong 1 trang. Mặc định là 20, tối đa 100.
        /// </summary>
        public int? PageSize { get; set; }

        /// <summary>
        /// Vị trí item hiện tại (sử dụng cho phân trang).
        /// </summary>
        public int? CurrentItem { get; set; }

        /// <summary>
        /// Trường dùng để sắp xếp (ví dụ: "name").
        /// </summary>
        public string OrderBy { get; set; }

        /// <summary>
        /// Chiều sắp xếp: "Asc" (mặc định) hoặc "Desc".
        /// </summary>
        public string OrderDirection { get; set; }

        /// <summary>
        /// Lấy danh sách Id bị xoá kể từ LastModifiedFrom.
        /// </summary>
        public bool? IncludeRemoveIds { get; set; }
    }
}
