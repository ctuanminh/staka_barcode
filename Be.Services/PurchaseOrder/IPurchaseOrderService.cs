using Be.Common.PurchaseOrder.Dto;
using Be.Common.PurchaseOrder.Request;
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
        Task<bool> AddPurchaseChecked(PurchaseCheckedDto purchaseCheckedDto);
        Task<bool> UpdatePurchaseChecked(long purchaseId, PurchaseCheckedDto purchaseCheckedDto);
        Task<List<PurchaseCheckedDto>> GetPurchaseCheckedByPurchaseId(long purchaseId);
        Task<PurchaseCheckedDto> GetPurchaseCheckedByProduct(long purchaseId, string productBarCode, long branchId);
    }
}
