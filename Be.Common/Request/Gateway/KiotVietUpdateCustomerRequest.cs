namespace Be.Common.Request.Gateway
{
    public class KiotVietUpdateCustomerRequest
    {
        public string Id { get; set; }
        public int Attempt { get; set; }
        public List<Notification> Notifications { get; set; }        
    }

    public class Notification
    {
        public string Action { get; set; }
        public List<CustomerData> Data { get; set; }
    }
    public class CustomerData 
    {
        public long Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public bool? Gender { get; set; }
        public DateTime? BirthDate { get; set; }
        public string ContactNumber { get; set; }
        public string Address { get; set; }
        public string LocationName { get; set; }
        public string Email { get; set; }
        public DateTime ModifiedDate { get; set; }
        public byte? Type { get; set; }
        public string Organization { get; set; }
        public string TaxCode { get; set; }
        public string Comments { get; set; }
    }
}
