using Microsoft.AspNetCore.Http;
using System;
using System.Security.Claims;

namespace Be.Services.BaseApp
{
    public class CurrentUserImp : ICurrentUser
    {
        // sử dụng IHttpContextAccessor để truy cập HttpContext
        // HttpContext chứa thông tin về người dùng hiện tại, bao gồm các claim, headers, cookies, v.v.         
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CurrentUserImp(IHttpContextAccessor httpContextAccessor) => _httpContextAccessor = httpContextAccessor;

        public long GetId()
        {
            var identifier = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            return !string.IsNullOrEmpty(identifier) ? long.Parse(identifier) : 0;
        }

        public long GetUserId()
        {
            var identifier = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            return !string.IsNullOrEmpty(identifier) ? long.Parse(identifier) : 0; 
        }

        public string GetIpAddress()
        {
            var identifier = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.Sid)?.Value;
            return identifier;
        }
        public string GetName() => _httpContextAccessor.HttpContext?.User.Identity.Name;
        public string GetEmail()
        {
            var identifier = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.Email)?.Value;
            return identifier;
        }
    }
}
