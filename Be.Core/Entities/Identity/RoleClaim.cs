using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace Be.Core.Entities.Identity
{
    public class RoleClaim : IdentityRoleClaim<long>
    {
        public override string ClaimType { get; set; }
        public override string ClaimValue { get; set; }

        [NotMapped]
        public long ClaimValueAsLong
        {
            get => long.TryParse(ClaimValue, out var val) ? val : 0;
            set => ClaimValue = value.ToString();
        }
    }
}
