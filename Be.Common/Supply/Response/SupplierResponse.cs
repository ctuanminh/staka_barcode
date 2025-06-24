using Be.Common.Supply.Dto;

namespace Be.Common.Supply.Response
{
    public class SupplierResponse
    {
        public long Id { get; set; }
        public long KiotId { get; set; } // ID nhà cung cấp trên KiotViet
        public string Code { get; set; }
        public string Name { get; set; }
        public string ContactNumber { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string LocationName { get; set; }
        public string WardName { get; set; }
        public string Organization { get; set; }
        public string TaxCode { get; set; }
        public string Comments { get; set; }
        public string Groups { get; set; }
        public bool IsActive { get; set; }
        public long RetailerId { get; set; }
        public long BranchId { get; set; }
        public string CreatedBy { get; set; }
        public decimal Debt { get; set; }
        public decimal TotalInvoiced { get; set; }
        public decimal TotalInvoicedWithoutReturn { get; set; }
    }
    public class SupplierPagedData
    {
        public List<int> RemovedId { get; set; } = new();  // Danh sách ID đã bị xóa
        public int Total { get; set; }                     // Tổng số nhà cung cấp
        public int PageSize { get; set; }                  // Số bản ghi 1 trang
        public List<SupplierResponse> Data { get; set; } = new();
    }
}
