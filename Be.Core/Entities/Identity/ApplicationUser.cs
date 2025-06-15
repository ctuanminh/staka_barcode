using Be.Core.BaseEntities;
using Be.Core.Entities.Crm;
using Microsoft.AspNetCore.Identity;

namespace Be.Core.Entities.Identity
{
	public class ApplicationUser : IdentityUser<long>, IAuditedEntity
    {
        public string FullName { get; set; }
        public string ResetPwToken { get; set; }
        public UserType UserType { get; set; }
        public bool? Employee { get; set; }
        public string EmployeeRole { get; set; }
        public string OwnerName { get; set; }
        public bool IsVendor { get; set; }
        public string VendorNumber { get; set; }
        public string Address { get; set; }
        public string RandomPw  { get; set; }
        public List<string> EmployeeRoles { get; set; }
        public Guid? DepartmentId { get; set; }
        public Department? Department { get; set; }

        public ICollection<Card> AssignedCards { get; set; } = [];
        public ICollection<Card> CreatedCards { get; set; } = [];
        public ICollection<CardAssignment> CardAssignments { get; set; } = [];

        public DateTime CreatedAt { get; set; }
        public long CreatedBy { get; set; }
        public DateTime UpdatedAt { get; set; }
        public long UpdatedBy { get; set; }
    }

	public enum UserType
	{
		Admin = 1,
		Employee = 2,
		User = 3,
		None = 0
	}
}
