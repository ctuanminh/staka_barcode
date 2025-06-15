using Microsoft.VisualBasic;

namespace Be.Common.customer
{
    public class InvoiceResponse
    {
        public int Id { get; set; }                     // ID hóa đơn
        public string Code { get; set; }                // Mã hóa đơn
        public string OrderCode { get; set; }          // Mã đơn hàng
        public DateTime PurchaseDate { get; set; }     // Ngày mua (có thể đổi sang DateTime nếu cần)
        public int BranchId { get; set; }               // ID chi nhánh
        public string BranchName { get; set; }          // Tên chi nhánh
        public int? CustomerId { get; set; }            // ID khách hàng
        public long SoldById { get; set; }
        public string SoldByName { get; set; }
        public string CustomerCode { get; set; }       // Mã khách hàng
        public string CustomerName { get; set; }       // Tên khách hàng
        public decimal Total { get; set; }              // Tổng tiền
        public decimal TotalPayment { get; set; }       // Tổng thanh toán
        public decimal? Discount { get; set; }          // Giảm giá
        public decimal? DiscountRatio { get; set; }     // Tỷ lệ giảm
        public string Description { get; set; }        // Ghi chú
        public int Status { get; set; }                 // Mã trạng thái
        public string StatusValue { get; set; }         // Tên trạng thái
        public bool UsingCod { get; set; }              // Sử dụng COD
        public List<InvoiceDetail> InvoiceDetails { get; set; } = new(); // Chi tiết hóa đơn
        public List<InvoicePayment> Payments { get; set; } = new();      // Thanh toán
        public InvoiceDelivery InvoiceDelivery { get; set; }            // Thông tin giao hàng
        public string CreatedDate { get; set; }         // Ngày tạo
        public string ModifiedDate { get; set; }
    }

    public class InvoiceDetail
    {
        public long ProductId { get; set; }                 // Id hàng hóa
        public string ProductCode { get; set; }             // Mã hàng hóa
        public string ProductName { get; set; }             // Tên hàng hóa (bao gồm thuộc tính và đơn vị tính)
        public double Quantity { get; set; }                // Số lượng hàng hóa
        public decimal Price { get; set; }                  // Giá trị
        public double? DiscountRatio { get; set; }          // Giảm giá theo %
        public decimal? Discount { get; set; }              // Giảm giá theo tiền
        public string Note { get; set; }                    // Ghi chú hàng hóa
        public string SerialNumbers { get; set; }
        public decimal SubTotal { get; set; }
    }

    public class InvoicePayment
    {
        public long Id { get; set; }                  // ID thanh toán
        public string Code { get; set; }              // Mã thanh toán
        public decimal Amount { get; set; }           // Số tiền
        public string Method { get; set; }            // Phương thức thanh toán
        public byte? Status { get; set; }             // Mã trạng thái (nullable)
        public string StatusValue { get; set; }       // Tên trạng thái
        public DateTime TransDate { get; set; }       // Ngày giao dịch
        public string BankAccount { get; set; }       // Tài khoản ngân hàng
        public int? AccountId { get; set; }
    }

    public class InvoiceDelivery
    {
        public string RecipientName { get; set; }       // Tên người nhận
        public string RecipientPhone { get; set; }      // Số điện thoại người nhận
        public string Address { get; set; }             // Địa chỉ giao hàng
        public string DeliveryDate { get; set; }        // Ngày giao hàng
        public string DeliveryStatus { get; set; }      // Trạng thái giao hàng
    }

    public class InvoiceApiResponse
    {
        public List<InvoiceResponse> Data { get; set; }
        public int Total { get; set; }
        public int PageSize { get; set; }
        public int CurrentItem { get; set; }
    }
}
