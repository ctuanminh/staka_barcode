using Be.Core.BaseEntities;

namespace Be.Core.Entities.InvoiceList
{
    public class Invoice : AuditedEntity
    {

        // --- NHÓM ĐỊNH DANH HÓA ĐƠN ---
        public string MauSo { get; set; }       // VD: 1/001
        public string KyHieu { get; set; }      // VD: C24TYY
        public string SoHoaDon { get; set; }    // VD: 00001234
        public DateTime NgayLap { get; set; }   // Ngày lập hóa đơn
        public DateTime? NgayKy { get; set; }   // Ngày ký số

        // --- MÃ QUẢN LÝ CỦA CƠ QUAN THUẾ
        public string MaCoQuanThue { get; set; } // Mã số duy nhất trên hệ thống thuế (MCCQT)
        public string MaTraCuu { get; set; }     // Dùng để tra cứu thủ công

        // --- THÔNG TIN NGƯỜI BÁN (VENDOR) ---
        
        public string MSTNguoiBan { get; set; }
        public string TenNguoiBan { get; set; }
        public string DiaChiNguoiBan { get; set; }

        // --- THÔNG TIN NGƯỜI MUA ---
       
        public string MSTNguoiMua { get; set; }
        public string TenNguoiMua { get; set; }

        // --- TỔNG CỘNG TIỀN (SUMMARY) ---
        public string LoaiTien { get; set; } = "VND"; 
        public decimal TyGia { get; set; } = 1;
        public decimal TongTienChuaThue { get; set; }
        public decimal TongTienThue { get; set; }
        public decimal TongTienThanhToan { get; set; }

        // --- TRẠNG THÁI ---
        public string TrangThai { get; set; } // VD: MOI, DA_HUY, DA_THAY_THE
        public string LinkFilePDF { get; set; } // Đường dẫn file PDF local
        public string LinkFileXML { get; set; } // Đường dẫn file XML local

        // Lưu trữ XML gốc
        public string RawXmlContent { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        //1 Hóa đơn có nhiều Hàng hóa
        public virtual ICollection<InvoiceItem> Items { get; set; } = new List<InvoiceItem>();
    }
}
