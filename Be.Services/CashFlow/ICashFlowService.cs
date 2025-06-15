using Be.Common.Dtos.CashFlow;
using Be.Common.Request;
using Be.Common.Responses;
using Be.Core.Entities;

namespace Be.Services.CashFlow
{
    public interface ICashFlowService
    {
        Task<ApiResponse> GetAllCashFlow(CashFlowRequest request);
        Task<ApiResponse> GetCashFlowByIdAsync(int id);
        Task<IEnumerable<ApiResponse>> GetCashFlowsAsync();
        Task<ApiResponse> CreateCashFlowAsync(Cashflow cashFlow);
        Task<ApiResponse> UpdateCashFlowAsync(Cashflow cashFlow);
        Task<bool> DeleteCashFlowAsync(int id);
        Task<byte[]> ExportMisaTemplate(SearchCashflowRequest request, string templatePath);
    }
}
