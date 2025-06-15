namespace Be.Common.Request.Gateway
{
    public class KiotVietUpdateInvoiceRequest
    {
        public string Id { get; set; }
        public int Attempt { get; set; }
        public List<InvoiceNotification> Notifications { get; set; }
        public class InvoiceNotification
        {
            public string Action { get; set; }
            public List<InvoiceData> Data { get; set; }
        }

        public class InvoiceData
        {
            public long Id { get; set; }
            public string Code { get; set; }
            public DateTime PurchaseDate { get; set; }
            public int BranchId { get; set; }
            public string BranchName { get; set; }
            public long SoldById { get; set; }
            public string SoldByName { get; set; }
            public long? CustomerId { get; set; }
            public string CustomerCode { get; set; }
            public string CustomerName { get; set; }
            public decimal Total { get; set; }
            public decimal TotalPayment { get; set; }
            public decimal? Discount { get; set; }
            public double? DiscountRatio { get; set; }
            public byte Status { get; set; } // 1: hoàn thành, 2: đã hủy, 3: đang xử lý, 5: không giao được
            public string StatusValue { get; set; }
            public string Description { get; set; }
            public bool UsingCod { get; set; }
            public DateTime? ModifiedDate { get; set; }

            public InvoiceDelivery InvoiceDelivery { get; set; }
            public List<InvoiceDetail> InvoiceDetails { get; set; }
            public List<InvoicePayment> Payments { get; set; }
        }

        public class InvoiceDelivery
        {
            public string DeliveryCode { get; set; }
            public byte Status { get; set; } // 1: chưa giao, 2: đang giao, 3: đã giao, v.v.
            public string StatusValue { get; set; }
            public byte? Type { get; set; }
            public decimal? Price { get; set; }
            public string Receiver { get; set; }
            public string ContactNumber { get; set; }
            public string Address { get; set; }
            public int? LocationId { get; set; }
            public string LocationName { get; set; }
            public double? Weight { get; set; }
            public double? Length { get; set; }
            public double? Width { get; set; }
            public double? Height { get; set; }
            public long? PartnerDeliveryId { get; set; }
            public PartnerDelivery PartnerDelivery { get; set; }
        }

        public class PartnerDelivery
        {
            public string Code { get; set; }
            public string Name { get; set; }
            public string ContactNumber { get; set; }
            public string Address { get; set; }
            public string Email { get; set; }
        }

        public class InvoiceDetail
        {
            public long ProductId { get; set; }
            public string ProductCode { get; set; }
            public string ProductName { get; set; }
            public double Quantity { get; set; }
            public decimal Price { get; set; }
            public decimal? Discount { get; set; }
            public double? DiscountRatio { get; set; }
        }

        public class InvoicePayment
        {
            public long Id { get; set; }
            public string Code { get; set; }
            public decimal Amount { get; set; }
            public int? AccountId { get; set; }
            public string BankAccount { get; set; }
            public string Description { get; set; }
            public string Method { get; set; }
            public byte? Status { get; set; }
            public string StatusValue { get; set; }
            public DateTime TransDate { get; set; }
        }

    }
}
