using Be.Core.BaseEntities;

namespace Be.Core.Entities.Identity
{
    public class UserSubRole : Entity<long>
    {
        public long UserId { get; set; }
        public long SubRoleId { get; set; }
    }
}
