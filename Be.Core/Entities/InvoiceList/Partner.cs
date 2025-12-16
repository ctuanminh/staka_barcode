using Be.Core.BaseEntities;
using System.ComponentModel.DataAnnotations;

namespace Be.Core.Entities.InvoiceList
{


    public class Partner : AuditedEntity
    {
        public string MaSoThue { get; set; }
        public string TenCongTy { get; set; }
        public string DiaChi { get; set; }
        public bool IsCustomer { get; set; }
        public bool IsVendor { get; set; }
    }


}
