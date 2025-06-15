using Be.Common.Request.Gateway;
using Be.Common.Responses;

namespace Be.Services.Gateway
{
    public interface IKiotVietService
    {
        public Task<ApiResponse> UpdateCustomer(KiotVietUpdateCustomerRequest request);
        public Task<ApiResponse> DeleteCustomer(int[] removeId);

        public Task<ApiResponse> UpdateProduct(KiotVietProductUpdateRequest request);
        public Task<ApiResponse> DeleteProduct(int[] removeId);

        public Task<ApiResponse> UpdateOrder(KiotVietUpdateOrderRequest request);

        public Task<ApiResponse> UpdateInvoince(KiotVietUpdateInvoiceRequest request);

        public Task<ApiResponse> UpdateStock(KiotVietUpdateStockRequest request);
        public Task<ApiResponse> DeleteStock(int[] removeId);

        public Task<ApiResponse> UpdatePriceBook(KiotVietUpdatePriceBookRequest request);
        public Task<ApiResponse> DeletePriceBook(int[] removeId);

        public Task<ApiResponse> UpdatePriceBookDetail(KiotVietUpdatePriceBookDetailRequest request);
        public Task<ApiResponse> DeletePriceBookDetail(KiotVietDeletePriceBookDetail request);

        public Task<ApiResponse> UpdateCategory(KiotVietUpdateCategory request);
        public Task<ApiResponse> DeleteCategory(int[] removeId);
        public Task<ApiResponse> UpdateBranch(KiotVietUpdateBranch request);
        Task<ApiResponse> DeleteBranch(int[] removeId);

    }
}
