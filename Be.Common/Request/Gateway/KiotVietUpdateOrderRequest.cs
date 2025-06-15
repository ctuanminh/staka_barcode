namespace Be.Common.Request.Gateway
{
    public class KiotVietUpdateOrderRequest
    {
        public string Id { get; set; }
        public int Attempt { get; set; }
        public List<OrderNotification> Notifications { get; set; }
        public class OrderNotification
        {
            public string Action { get; set; }
            public List<OrderData> Data { get; set; }
        }

        public class OrderData
        {
            public long Id { get; set; }
            public string Code { get; set; }
            public DateTime PurchaseDate { get; set; }
            public int BranchId { get; set; }
            public long? SoldById { get; set; }
            public string SoldByName { get; set; }
            public long? CustomerId { get; set; }
            public string CustomerCode { get; set; }
            public string CustomerName { get; set; }
            public decimal Total { get; set; }
            public decimal TotalPayment { get; set; }
            public decimal? Discount { get; set; }
            public double? DiscountRatio { get; set; }
            public int Status { get; set; }
            public string StatusValue { get; set; }
            public string Description { get; set; }
            public bool UsingCod { get; set; }
            public DateTime? ModifiedDate { get; set; }

            public List<OrderDetail> OrderDetails { get; set; }
        }

        public class OrderDetail
        {
            public long ProductId { get; set; }
            public string ProductCode { get; set; }
            public string ProductName { get; set; }
            public double Quantity { get; set; }
            public decimal Price { get; set; }
            public decimal? Discount { get; set; }
            public double? DiscountRatio { get; set; }
        }
    }
}
