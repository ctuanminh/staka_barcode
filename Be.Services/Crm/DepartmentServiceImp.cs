using Be.Common.Dtos.Crm;
using Be.Common.Responses;

namespace Be.Services.Crm
{
    public interface DepartmentServiceImp : IDepartmentService
    {
        public virtual Task<ApiResponse> GetDepartmentByIdAsync(int id)
        {
            throw new NotImplementedException();
        }
        public virtual Task<IEnumerable<ApiResponse>> GetDepartmentsAsync()
        {
            throw new NotImplementedException();
        }
        public virtual Task<ApiResponse> CreateDepartmentAsync(DepartmentRequest department)
        {
            throw new NotImplementedException();
        }
        public virtual Task<ApiResponse> UpdateDepartmentAsync(DepartmentRequest department)
        {
            throw new NotImplementedException();
        }
        public virtual Task<bool> DeleteDepartmentAsync(int id)
        {
            throw new NotImplementedException();
        }
    }
}
