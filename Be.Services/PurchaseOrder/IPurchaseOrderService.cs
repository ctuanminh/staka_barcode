using Be.Common.Purchase_Order.Dto;
using Be.Common.Purchase_Order.Request;
using Be.Common.Responses;

namespace Be.Services.PurchaseOrder
{
    public interface IPurchaseOrderService
    {
        Task<ApiResponse> GetAllPurchaseOrders(SearchPurchaseOrderRequest purchaseOrderRequest);
        Task<byte[]> ExportPurchaseOrderMisa(SearchPurchaseOrderRequest purchaseOrderRequest, string templatePath);
        Task<ApiResponse> ImportPurchaseOrderMisa(Stream fileStream, string fileName);
        Task<ApiResponse> GetPurchaseOrderById(Guid id);
        Task<ApiResponse> CreatePurchaseOrder(PurchaseOrderDto purchaseOrderDto);
        Task<ApiResponse> UpdatePurchaseOrder(Guid id, PurchaseOrderDto purchaseOrderDto);
        Task<ApiResponse> DeletePurchaseOrder(Guid id);
    }
}
