using Be.Core.BaseEntities;
using Be.Core.Entities.Identity;

namespace Be.Core.Entities.Crm
{
    public class CardAssignment : AuditedEntity
    {
        public Guid CardId { get; set; }
        public Card Card { get; set; } = null!;

        public long UserId { get; set; }
        public ApplicationUser User { get; set; } = null!;

        public DateTime AssignedAt { get; set; } = DateTime.UtcNow;
    }
}
