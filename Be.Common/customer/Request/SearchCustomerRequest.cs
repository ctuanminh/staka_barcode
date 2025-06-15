namespace Be.Common.customer.Request
{
    public class SearchCustomerRequest
    {
        public string Name { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string orderBy { get; set; } //Sắp xếp dữ liệu theo trường orderBy (Ví dụ: orderBy=name)
        public string orderDirection { get; set; } //Sắp xếp kết quả trả về theo: Tăng dần Asc (Mặc định), giảm dần Desc
        public bool includeRemoveIds { get; set; }
        public bool includeTotal { get; set; } //Có lấy thông tin TotalInvoice, TotalPoint, TotalRevenue
        public bool includeCustomerGroup { get; set; } //Có lấy thông tin nhóm khách hàng hay không
        public int currentItem { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }
}
