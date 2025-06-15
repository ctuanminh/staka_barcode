namespace Be.Common.Request.Gateway
{
    public class KiotVietDeletePriceBookDetail
    {
        public string Id { get; set; }
        public int Attempt { get; set; }
        public List<PriceBookDetailDeleteNotification> Notifications { get; set; }
    }

    public class PriceBookDetailDeleteNotification
    {
        public string Action { get; set; }
        public List<PriceBookDetailDeleteData> Data { get; set; }
    }

    public class PriceBookDetailDeleteData
    {
        public long PricebookId { get; set; }       // Id bảng giá
        public List<long> ProductIds { get; set; }  // Danh sách ID hàng hóa bị xóa khỏi bảng giá
    }
}
