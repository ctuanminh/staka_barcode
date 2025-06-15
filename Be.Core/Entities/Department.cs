using Be.Core.BaseEntities;
using Be.Core.Entities.Identity;

namespace Be.Core.Entities
{
    public class Department : AuditedEntity
    {
        public string Name { get; set; } = string.Empty;

        public Guid BranchId { get; set; }
        public Branch Branch { get; set; } = null!;

        public ICollection<ApplicationUser> Users { get; set; } = new List<ApplicationUser>();
        public ICollection<Board> Boards { get; set; } = new List<Board>();
    }
}
