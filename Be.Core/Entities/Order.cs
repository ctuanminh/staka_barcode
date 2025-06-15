using System.ComponentModel.DataAnnotations;
using Be.Core.BaseEntities;

namespace Be.Core.Entities
{
    public class Order : AuditedEntity
    {
        [Required]
        public Guid UserId { get; set; }
        public string FullName { get; set; }

        [Required]
        public string Email { get; set; }
        public string Address { get; set; }
        
        [Required]
        public string PhoneNumber { get; set; }
        public DateTime OrderDate { get; set; }
        public float TotalMoney { get; set; }
        public string ShippingMethod { get; set; }
        public string ShippingAddress { get; set; }
        public DateTime ShippingDate { get; set; }
        public string PaymentMethod { get; set; }

        public List<OrderDetail> OrderDetails { get; set; }

    }
}
