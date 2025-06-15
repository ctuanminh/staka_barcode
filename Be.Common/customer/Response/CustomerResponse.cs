namespace Be.Common.customer.Response
{
    public class CustomerResponse
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public bool Gender { get; set; } // Sửa từ string thành bool        
        public string GenderDisplay => Gender ? "Male" : "Female"; // Thêm thuộc tính để hiển thị
        public DateTime BirthDate { get; set; }
        public string ContactNumber { get; set; }
        public string Address { get; set; }
        public int RetailerId { get; set; }
        public int BranchId { get; set; }
        public string BranchName { get; set; }
        public decimal TotalInvoiced { get; set; }
        public long TotalPoint { get; set; }
        public decimal TotalRevenue { get; set; }
        public decimal Debt { get; set; }
        public string Groups { get; set; }
        public string LocationName { get; set; }
        public string WardName { get; set; }
        public DateTime ModifiedDate { get; set; }
        public DateTime CreatedDate { get; set; }

    }
    public class CustomerPagedResponse
    {
        public List<CustomerResponse> Data { get; set; }
        public int Total { get; set; }
        public int PageSize { get; set; }
        public int CurrentItem { get; set; }
    }
}
