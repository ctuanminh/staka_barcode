using Be.Common.customer.Request;
using Be.Common.Dtos.Invoice;
using Be.Common.Responses;

namespace Be.Services.customer
{
    public interface ICustomerService
    {
        Task<ApiResponse> GetAllCustomer(SearchCustomerRequest request);
        Task<ApiResponse> ExportInvoiceMisa(SearchInvoiceRequest request, string templatePath);
        Task<bool> SyncCustomer();
    }
}
