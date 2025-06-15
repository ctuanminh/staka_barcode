
using Be.Core.Entities.Identity;
using System.ComponentModel.DataAnnotations;

namespace Be.Common.Dtos.Identity
{
	public class UserRequest
	{
		[Required(ErrorMessage = "The {0} is  required")]
        public string Email { get; set; }
        public string Password { get; set; }
        public string PhoneNumber { get; set; }
        public string EmployeeRole { get; set; }
        public string OwnerName { get; set; }
        public UserType UserType { get; set; }

		[Required(ErrorMessage = "Roles is required!")]
		public List<long> RolesIds { get; set; }
		public List<string> EmployeeRoles { get; set; }
        public bool IsVendor { get; set; }

	}
}
