using Be.Common.Dtos.Identity;
using Be.Common.Request;
using Be.Common.Responses;
using Be.Common.User.Dto;

namespace Be.Services.Identity
{
    public interface IUserService
    {
        Task<(bool success, UserLoginDto userLoginDto)> Login(UserLoginRequest request);
        Task<ApiResponse> GetAllUsers(SearchUserRequest request);
        Task<ApiResponse> GetUserById(Guid id);
        Task<ApiResponse> GetAllRoles();
        Task<ApiResponse> AddUser(UserRequest request);
        Task<ApiResponse> AddRole(CreateRoleRequest request);
        Task<ApiResponse> SyncRole(SyncRoleRequest request);
        Task<bool> SyncUser(SyncUserRequest request);
        Task<ApiResponse> GetInfo(string token);
    }
}