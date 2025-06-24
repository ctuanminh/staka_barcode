using Be.Core.BaseEntities;

namespace Be.Core.Entities
{
    public class TransferEntity : AuditedEntity
    {
        public string TransferCode { get; set; }
        public long TransferId { get; set; }
        public int Status { get; set; }
        public long FromBranchId { get; set; }
        public long ToBranchId { get; set; }
    }
}
