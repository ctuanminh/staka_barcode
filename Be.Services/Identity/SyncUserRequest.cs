namespace Be.Services.Identity
{
    public class SyncUserRequest
    {
        /// <summary>
        /// Thời gian cập nhật gần nhất để lọc dữ liệu.
        /// </summary>
        public DateTime? LastModifiedFrom { get; set; }

        /// <summary>
        /// Số lượng item mỗi trang (mặc định 20, tối đa 100).
        /// </summary>
        public int? PageSize { get; set; }

        /// <summary>
        /// Vị trí bắt đầu lấy dữ liệu (index-based).
        /// </summary>
        public int? CurrentItem { get; set; }

        /// <summary>
        /// Trường cần sắp xếp (ví dụ: name).
        /// </summary>
        public string OrderBy { get; set; }

        /// <summary>
        /// Hướng sắp xếp: Asc (mặc định) hoặc Desc.
        /// </summary>
        public string OrderDirection { get; set; }

        /// <summary>
        /// Có lấy danh sách ID đã bị xoá không.
        /// </summary>
        public bool? IncludeRemoveIds { get; set; }
    }
}