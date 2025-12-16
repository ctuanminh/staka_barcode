using Be.Core.BaseEntities;

namespace Be.Core.Entities.InvoiceList
{
    public class InvoiceItem : AuditedEntity
    {
        public string InvoiceId { get; set; }
       
        public virtual Invoice Invoice { get; set; }

        public int STT { get; set; } // Số thứ tự dòng

        public string MaHang { get; set; }

        public string TenHang { get; set; }

        public string DonViTinh { get; set; }

        public decimal SoLuong { get; set; }
        public decimal DonGia { get; set; }
        public decimal ThanhTien { get; set; } // = Số lượng * Đơn giá

        // --- THÔNG TIN THUẾ TỪNG DÒNG ---
        public string ThueSuat { get; set; }
        public decimal TienThue { get; set; }
    }
}
