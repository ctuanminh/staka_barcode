using Be.Core.BaseEntities;

namespace Be.Core.Entities.Identity;

public class ActionInFunctionEntity : AuditedEntity
{
    public long FunctionId { get; set; }
    public long  ActionId { get; set; }
    public ActionEntity ActionEntity { get; set; }
    public FunctionEntity FunctionEntity { get; set; }
}