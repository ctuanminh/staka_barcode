namespace Be.Common.Supply.Dto
{
    public class SupplierDto
    {
        public long KiotId { get; set; }                 // ID nhà cung cấp trên KiotViet
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
        public DateTime ModifiedDate { get; set; }
        public DateTime CreatedDate { get; set; }
        public long RetailerId { get; set; }
        public long BranchId { get; set; }
        public string CreatedBy { get; set; }
        public decimal Debt { get; set; }
        public decimal TotalInvoiced { get; set; }
        public decimal TotalInvoicedWithoutReturn { get; set; }
    }
}
