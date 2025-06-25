namespace Be.Common.PurchaseOrder.Dto
{
    public class PurchaseCheckedDto
    {
        public long PurchaseId { get; set; }
        public string PurchaseCode { get; set; }
        public string ProductBarCode { get; set; }
        public long BranchId { get; set; }
        public string UserName { get; set; }
        public bool Checked { get; set; }
    }
}
