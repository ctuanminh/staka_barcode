using Be.Core.BaseEntities;

namespace Be.Core.Entities
{
    public class TransferChecked : AuditedEntity
    {
        public long TransferId { get; set; }
        public string TransferCode { get; set; }
        public string ProductBarCode { get; set; }
        public long BranchId { get; set; }
        public string  UserName { get; set; }
        public bool Checked { get; set; }
    }
}
