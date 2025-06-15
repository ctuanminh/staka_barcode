namespace Be.Common.Responses.KiotViet
{
    public class KiotVietRoleApiResponse
    {
        public int IdOld { get; set; }
        public string CompareName { get; set; }
        public long Id { get; set; }
        public string Name { get; set; }
        public long RetailerId { get; set; }
        public DateTime CreatedDate { get; set; }
        public bool IsActive { get; set; }
        public List<string> Permissions { get; set; } = new();
        public List<string> ObjectAccesses { get; set; } = new();
        public List<string> PropertyAccesses { get; set; } = new();
    }
}
