using Be.Core.BaseEntities;

namespace Be.Core.Entities
{
    public class OrderDetail : AuditedEntity
    {
        public Guid OrderId { get; set; }
        public Guid ProductId { get; set; }
        public float Price { get; set; }
        public int NumberOfProduct { get; set; }
        public float TotalMoney { get; set; }
        public string Color { get; set; }

        public Order Order { get; set; }
    }
}
