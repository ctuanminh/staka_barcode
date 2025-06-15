using Be.Core.BaseEntities;

namespace Be.Core.Entities.Identity
{
    public class FunctionEntity : AuditedEntity
    {
        public string Name { get; set; }
        public string Url { get; set; }
        public long ParentId { get; set; }
        public bool IsDeleted { get; set; }
        public ICollection<ActionInFunctionEntity> ActionInFunctionEntities { get; set; }
    }
}
