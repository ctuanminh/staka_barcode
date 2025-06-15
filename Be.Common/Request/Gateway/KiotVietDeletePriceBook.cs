namespace Be.Common.Request.Gateway
{
    public class KiotVietDeletePriceBook
    {
        public string Id { get; set; }
        public int Attempt { get; set; }
        public List<PriceBookDeleteNotification> Notifications { get; set; }

    }
    public class PriceBookDeleteNotification
    {
        public string Action { get; set; }
        public List<long> Data { get; set; } // Danh sách Id bảng giá bị xoá
    }
}
