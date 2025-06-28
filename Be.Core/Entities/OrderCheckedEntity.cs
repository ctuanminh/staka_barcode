using Be.Core.BaseEntities;

namespace Be.Core.Entities
{
    public class OrderCheckedEntity : AuditedEntity
    {
        public string OrderCode { get; set; }
        public long OrderId { get; set; }
        public string ProductCode { get; set; }
        public string ProductBarCode { get; set; }
        public long BranchId { get; set; }
        public string UserName { get; set; }
        public double Count { get; set; }

    }
}
