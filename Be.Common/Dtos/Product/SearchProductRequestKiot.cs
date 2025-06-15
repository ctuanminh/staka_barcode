namespace Be.Common.Dtos.Product
{
    public class SearchProductRequestKiot
    {
        public int[] BranchIds { get; set; }            // ID chi nhánh (optional)        
        public string Code { get; set; }                // Mã khách hàng
        public int PageSize { get; set; }              // Số items trong 1 trang, mặc định 20 items, tối đa 100 items
        public int CurrentItem { get; set; }
        public bool IsActive { get; set; } // hàng đang kinh danh
        public bool includePricebook { get; set; } = false; // Thông tin bảng giá
        public bool includeWarranties { get; set; } = false; //Lấy thông tin bảo hành
        public bool includeRemoveIds { get; set; } = false; //Lấy thông tin hàng hoá xoá
        public bool includeQuantity { get; set; } = false;
        public bool includeMaterial { get; set; } = false;

    }
}
