namespace Be.Common.Dtos.Identity
{
    public class SyncRoleRequest
    {
        public string? LastModifiedFrom { get; set; }
        public int? PageSize { get; set; }
        public int? CurrentItem { get; set; }
        public string? OrderBy { get; set; }
        public string? OrderDirection { get; set; } // Giá trị "ASC" hoặc "DESC"
        public bool? IncludeRemoveIds { get; set; }
    }
}
