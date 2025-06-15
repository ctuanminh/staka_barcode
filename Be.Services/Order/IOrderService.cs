using Be.Common.Dtos.Invoice;
using Be.Common.Responses;

namespace Be.Services.Order
{
    public interface IOrderService
    {
        Task<ApiResponse> GetAllOrder(SearchInvoiceRequest request);
        Task<ApiResponse> GetAllInvoice(SearchInvoiceRequest request, string templatePath);
        Task<Byte[]> ExportInvoiceMisa(SearchInvoiceRequest request, string templatePath);       
    }
}
