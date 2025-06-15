using Be.Common.Responses;

namespace Be.Services.Crm
{
    public interface IDepartmentService
    {
        Task<ApiResponse> GetDepartmentByIdAsync(int id);
        Task<IEnumerable<ApiResponse>> GetDepartmentsAsync();
        Task<ApiResponse> CreateDepartmentAsync(DepartmentDto department);
        Task<ApiResponse> UpdateDepartmentAsync(DepartmentDto department);
        Task<bool> DeleteDepartmentAsync(int id);
    }
}
