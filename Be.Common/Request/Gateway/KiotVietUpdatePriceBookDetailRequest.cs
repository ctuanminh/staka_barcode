namespace Be.Common.Request.Gateway
{
    public class KiotVietUpdatePriceBookDetailRequest
    {
        public string Id { get; set; }
        public int Attempt { get; set; }
        public List<PriceBookDetailUpdateNotification> Notifications { get; set; }
        public class PriceBookDetailUpdateNotification
        {
            public string Action { get; set; }
            public List<PriceBookDetailData> Data { get; set; }
        }

        public class PriceBookDetailData
        {
            public long PriceBookId { get; set; }  // Id bảng giá
            public long ProductId { get; set; }    // Id hàng hóa
            public decimal Price { get; set; }     // Giá áp dụng
        }
    }
}
