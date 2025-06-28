namespace Be.Common.Order.Dto
{
    public class OrderCheckedDto
    {
        public long Id { get; set; }
        public string OrderCode { get; set; }
        public long OrderId { get; set; }
        public string ProductCode { get; set; }
        public string ProductBarCode { get; set; }
        public long BranchId { get; set; }
        public string UserName { get; set; }
        public double Count { get; set; }
    }
}
