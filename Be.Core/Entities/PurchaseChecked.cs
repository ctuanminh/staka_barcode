using Be.Core.BaseEntities;

namespace Be.Core.Entities
{
    public class PurchaseCheckedEntity : AuditedEntity
    {
        public long PurchaseId { get; set; }
        public string PurchaseCode { get; set; }
        public string ProductBarCode { get; set; }
        public string ProductCode { get; set; }
        public long BranchId { get; set; }
        public string UserName { get; set; }
        public bool Checked { get; set; }
    }
}
