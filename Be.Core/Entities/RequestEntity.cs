using Be.Core.BaseEntities;

namespace Be.Core.Entities
{
    public class RequestEntity : AuditedEntity
    {
        public string Module { get; set; }
        public string Url { get; set; }
        public bool IsSuccess { get; set; }
        public long? BranchId { get; set; }
    }
}
