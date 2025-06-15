namespace Be.Common.Branch.Response
{
    public partial class BranchResponse
    {
        public int Id { get; set; }
        public int BranchId { get; set; }
        public int RetailerId { get; set; }
        public string BranchName { get; set; }
        public string Address { get; set; }
        public string ContactNumber { get; set; }
        public int Status { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }
    }

    public partial class BranchPagedResponse
    {
        public List<BranchResponse> Data { get; set; }
        public int Total { get; set; }
        public int PageSize { get; set; }
        public int CurrentItem { get; set; }
    }
}
