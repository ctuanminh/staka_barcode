using Be.Core.BaseEntities;

namespace Be.Core.Entities
{
    public class BankAccountEntity : AuditedEntity<long>
    {
        public long KiotId { get; set; }
        public string BankName { get; set; } // Tên tài khoản ngân hàng

        public string AccountNumber { get; set; } // Số tài khoản ngân hàng

        public string Description { get; set; } // Ghi chú

        public int RetailerId { get; set; } // ID cửa hàng
    }
}
