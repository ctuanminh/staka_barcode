using Be.Common.Request.KiotViet;
using Be.Common.Responses;

namespace Be.Services.Crm
{
    public interface IReportService
    {
        Task<ApiResponse> GetSellReport(ReportSellRequest reportSellRequest);
        Task<ApiResponse> GetCustomerReport(ReportCustomerRequest reportCustomerRequest);
        Task<Byte[]> ExportSellReport(ReportSellRequest reportSellRequest, string templatePath);
        Task<string> LoginAndGetTokenAsync(string username, string password);
        Task<Byte[]> ExportCustomerReport(ReportCustomerRequest reportCustomerRequest, string templatePath);
    }
}
