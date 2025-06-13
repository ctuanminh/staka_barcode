using System;
using System.Collections.Generic;

namespace FrmMain.Dto.Response
{
    public class OrderResponse
    {
        public long Id { get; set; }
        public string Code { get; set; }
        public DateTime PurchaseDate { get; set; }
        public long BranchId { get; set; }
        public string BranchName { get; set; }
        public long SoldById { get; set; }
        public string SoldByName { get; set; }
        public long CustomerId { get; set; }
        public string CustomerCode { get; set; }
        public string CustomerName { get; set; }
        public decimal Total { get; set; }
        public decimal TotalPayment { get; set; }
        public decimal Discount { get; set; }
        public int DiscountRatio { get; set; }
        public int Status { get; set; }
        public string StatusValue { get; set; }
        public long RetailerId { get; set; }
        public string Description { get; set; }
        public bool UsingCod { get; set; }
        public DateTime CreatedDate { get; set; }
        public long PriceBookId { get; set; }
        public string Extra { get; set; }  // bạn có thể dùng JObject nếu muốn parse sâu hơn
        public List<OrderDetail> OrderDetails { get; set; }
    }
    public class OrderDetail
    {
        public long ProductId { get; set; }
        public string ProductCode { get; set; }
        public string ProductName { get; set; }
        public bool IsMaster { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public decimal Discount { get; set; }
        public int DiscountRatio { get; set; }
        public string Note { get; set; }
        public decimal ViewDiscount { get; set; }
    }

}
