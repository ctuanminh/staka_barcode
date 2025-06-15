using Be.Core.BaseEntities;
using Be.Core.Entities.Crm;
using Microsoft.AspNetCore.Identity;

namespace Be.Core.Entities.Identity
{
    public class User : IdentityUser<long>, IEntity<long>
    {
        public bool? Employee { get; set; }
        public ICollection<Card> Cards { get; set; } = [];
        public ICollection<CardAssignment> CardAssignments { get; set; } = [];
    }
}
