using Be.Core.BaseEntities;

namespace Be.Core.Entities
{
    public class Branch : AuditedEntity
    {
        public string BranchCode { get; set; }
        public int BranchId { get; set; }     
        public int RetailerId { get; set; }
        public string BranchName { get; set; }
        public string Address { get; set; }
        public string ContactNumber { get; set; }
        public int Status { get; set; }
        //Tài khoản kho
        public string InventoryAccount { get; set; }
        // Tải khoản giá vốn
        public string CostAccount { get; set; }

    }
}
