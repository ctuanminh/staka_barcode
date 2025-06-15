using System.Security.AccessControl;

namespace Be.Common.Request
{
    public class SearchUserRequest : SearchRequest
    {
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Name { get; set; }
    }
}
