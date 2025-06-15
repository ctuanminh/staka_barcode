using Be.Core.BaseEntities;

namespace Be.Core.Entities.KiotViet
{
    public class ApiLog : AuditedEntity
    {        
        public string Module { get; set; }
        public string Content { get; set; }
        public string Status { get; set; }
    }
}
