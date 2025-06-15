using Be.Core.Entities.Identity;

namespace Be.Common.Responses
{
    public class UserModelResponse
    {
        public long Id { get; set; }
        public string UserName { get; set; }
        public UserType UserType { get; set; }
        public string Email { get; set; }
        public bool Lock { get; set; }
        public string Phone { get; set; }
        public List<long> RolesId { get; set; }

    }
}
