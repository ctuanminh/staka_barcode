namespace Be.Common.Request.Gateway
{
    public class KiotVietUpdateBranch
    {
        public string Id { get; set; }
        public int Attempt { get; set; }
        public List<BranchUpdateNotification> Notifications { get; set; }
    }
    public class BranchUpdateNotification
    {
        public string Action { get; set; }
        public List<BranchUpdateData> Data { get; set; }
    }

    public class BranchUpdateData
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string ContactNumber { get; set; }
        public string SubContactNumber { get; set; }
        public string Address { get; set; }
        public string Location { get; set; }
        public string WardName { get; set; }
        public bool IsActive { get; set; }
        public bool IsLock { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public int RetailerId { get; set; }
    }
}
