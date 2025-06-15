using Be.Core.BaseEntities;
using Be.Core.Entities.Crm;

namespace Be.Core.Entities
{
    public class Board : AuditedEntity
    {
        public string Name { get; set; } = string.Empty;

        public Guid DepartmentId { get; set; }
        public Department Department { get; set; } = null!;
        public ICollection<TaskList> Lists { get; set; } = new List<TaskList>();
    }
}
