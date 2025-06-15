using Be.Core.BaseEntities;

namespace Be.Core.Gateway
{
    public class WorkQueue : AuditedEntity
    {
        public string Source { get; set; } = string.Empty; // Ví dụ: "KiotViet"

        public string Module { get; set; } = string.Empty; // Ví dụ: "Invoice", "Customer"

        public string Payload { get; set; } = string.Empty; // JSON content

        public string Status { get; set; } = "Queued"; // Queued, Success, Failed

        public int RetryCount { get; set; } = 0;

        public string? LastError { get; set; }
    }
}
