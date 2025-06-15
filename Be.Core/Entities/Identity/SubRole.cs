using Be.Core.BaseEntities;

namespace Be.Core.Entities.Identity
{
    public class SubRole : Entity<long>
    {
        public string Name { get; set; }
        public string  Description { get; set; }
        public long RoleId { get; set; }

    }
}
