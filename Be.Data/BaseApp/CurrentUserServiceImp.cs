using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;

namespace Be.Data.BaseApp
{
    public class CurrentUserServiceImp : ICurrentUserService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CurrentUserServiceImp(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public string GetEmail()
        {
            var email = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.Email)?.Value;
            return email;
        }
        public Guid GetId()
        {
            var identifier = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            return identifier.IsNullOrEmpty() ? Guid.Empty : new Guid(identifier);
        }
        public string GetIp()
        {
            var identifier = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.Sid)?.Value;
            return identifier;
        }
        public string GetName()
        {
            var name = _httpContextAccessor.HttpContext?.User?.Identity?.Name;
            return name;
        }
        public string GetUserId()
        {
            var identifier = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            return identifier;
        }

    }
}