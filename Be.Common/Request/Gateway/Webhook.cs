namespace Be.Common.Request.Gateway
{
    public class Webhook
    {
        public string Type { get; set; }            // Kiểu sự kiện, ví dụ: "customer.update"
        public string Url { get; set; }             // Endpoint được webhook gửi đến
        public bool IsActive { get; set; }          // Trạng thái bật/tắt
        public string Description { get; set; }     // Mô tả webhook
        public string Secret { get; set; }
    }
}
