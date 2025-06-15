using Be.Core.BaseEntities;
using Be.Core.Entities.Identity;

namespace Be.Core.Entities.Crm
{
    public class Card : AuditedEntity
    {
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public DateTime? DueDate { get; set; }
        public string? Status { get; set; }

        public Guid TaskListId { get; set; }
        public TaskList TaskList { get; set; } = null!;

        public long? AssignedUserId { get; set; } // nullable vì có thể bị set null khi user bị xóa
        public ApplicationUser? AssignedUser { get; set; }
        public Guid CreatedById { get; set; }

        public ICollection<CardAssignment> Assignments { get; set; } = [];
    }
}
