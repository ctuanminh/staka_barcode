namespace Be.Common.Supply.Request
{
    public class SearchSupplierRequest
    {
        public int? PageSize { get; set; }            // Số bản ghi trong 1 trang (mặc định 20, tối đa 100)
        public int? CurrentItem { get; set; }         // Vị trí bắt đầu lấy dữ liệu (mặc định lấy từ bản ghi số 1)
        public string? OrderDirection { get; set; }   // Sắp xếp (Asc | Desc)
        public string? Code { get; set; }             // Tìm kiếm theo mã nhà cung cấp
        public string? Name { get; set; }             // Tìm kiếm theo tên nhà cung cấp
        public string? ContactNumber { get; set; }    // Tìm kiếm theo điện thoại
        public DateTime? LastModifiedFrom { get; set; }  // Tìm kiếm theo thời gian cập nhật
        public bool? IncludeRemoveIds { get; set; }   // Có lấy thông tin danh sách ID bị xóa
        public bool? IncludeTotal { get; set; }       // Có lấy totalInvoiced, totalInvoicedWithoutReturn
        public bool? IncludeSupplierGroup { get; set; } // Có lấy thông tin Groups
    }
}
