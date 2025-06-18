namespace Be.Common.Order.Request
{
    public class OrderKiotRequest
    {
        public DateTime purchaseDate { get; set; }
        public int branchId { get; set; }
        public long? soldById { get; set; }
        public long? cashierId { get; set; }
        public decimal discount { get; set; }
        public string description { get; set; }
        public string method { get; set; }
        public decimal totalPayment { get; set; }
        public int? accountId { get; set; }
        public bool makeInvoice { get; set; }
        public int? saleChannelId { get; set; }
        public List<OrderDetail> orderDetails { get; set; }
        public OrderDelivery orderDelivery { get; set; }
        public Customer customer { get; set; }
        public List<Surcharge> surchages { get; set; }
    }
    public class OrderDetail
    {
        public long productId { get; set; }
        public string productCode { get; set; }
        public string productName { get; set; }
        public bool isMaster { get; set; }
        public double quantity { get; set; }
        public decimal price { get; set; }
        public decimal? discount { get; set; }
        public double? discountRatio { get; set; }
    }
    public class OrderDelivery
    {
        public string deliveryCode { get; set; }
        public byte? type { get; set; }
        public decimal? price { get; set; }
        public string receiver { get; set; }
        public string contactNumber { get; set; }
        public string address { get; set; }
        public int? locationId { get; set; }
        public string locationName { get; set; }
        public string wardName { get; set; }
        public double? weight { get; set; }
        public double? length { get; set; }
        public double? width { get; set; }
        public double? height { get; set; }
        public long? partnerDeliveryId { get; set; }
        public DateTime expectedDelivery { get; set; }
        public PartnerDelivery partnerDelivery { get; set; }
    }
    public class PartnerDelivery
    {
        public string code { get; set; }
        public string name { get; set; }
        public string address { get; set; }
        public string contactNumber { get; set; }
        public string email { get; set; }
    }
    public class Customer
    {
        public long id { get; set; }
        public string code { get; set; }
        public string name { get; set; }
        public bool gender { get; set; }
        public DateTime birthDate { get; set; }
        public string contactNumber { get; set; }
        public string address { get; set; }
        public string wardName { get; set; }
        public string email { get; set; }
        public string comments { get; set; }
    }

    public class Surcharge
    {
        public int id { get; set; }
        public string code { get; set; }
        public decimal price { get; set; }
    }
}
