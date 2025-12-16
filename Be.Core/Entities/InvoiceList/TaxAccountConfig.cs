using Be.Core.BaseEntities;

namespace Be.Core.Entities.InvoiceList
{
    public class TaxAccountConfig : AuditedEntity
    {
        
        public string MaSoThue { get; set; } 
        public string TenCongTy { get; set; }
        public string TaxUsername { get; set; }
        public string TaxPassword { get; set; }
        public DateTime LastSync { get; set; }
        public bool IsActive { get; set; }
    }


}
