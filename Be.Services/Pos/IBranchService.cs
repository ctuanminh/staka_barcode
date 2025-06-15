using Be.Common.Branch.Request;
using Be.Common.Responses;

namespace Be.Services.Pos
{
    public interface IBranchService
    {
        Task<ApiResponse> SyncBranch(BranchRequest branchRequest);
        Task<ApiResponse> GetAllBranches();
        Task<ApiResponse> GetBranchById(int id);
        Task<ApiResponse> CreateBranch(BranchRequest branchRequest);
        Task<ApiResponse> UpdateBranch(BranchRequest branchRequest);
        Task<ApiResponse> DeleteBranch(int id);
        Task<ApiResponse> GetBranchesByCompany(int companyId);
        Task<ApiResponse> GetBranchesByUser(int userId);
    }
}
