using Be.Core.BaseEntities;

namespace Be.Core.Entities.Identity;

public class ActionEntity : AuditedEntity
{
    public string Name { get; set; }
    public bool IsDeleted { get; set; }
}