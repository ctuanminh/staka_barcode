using Be.Common.Gateway.Misa;
using Be.Common.Request.Gateway;
using Be.Common.Responses;
using MassTransit;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Be.Services.Gateway
{
    public class KiotVietServiceImp : ServiceResponse, IKiotVietService
    {
        private readonly IPublishEndpoint _publishEndpoint;

        public KiotVietServiceImp(IPublishEndpoint publishEndpoint)
        {
            _publishEndpoint = publishEndpoint;
        }

        public Task<ApiResponse> UpdateCustomer(KiotVietUpdateCustomerRequest request)
        {
            throw new NotImplementedException();
        }

        public Task<ApiResponse> DeleteCustomer(int[] removeId)
        {
            throw new NotImplementedException();
        }

        public Task<ApiResponse> UpdateProduct(KiotVietProductUpdateRequest request)
        {
            throw new NotImplementedException();
        }

        public Task<ApiResponse> DeleteProduct(int[] removeId)
        {
            throw new NotImplementedException();
        }


        public async Task<ApiResponse> UpdateOrder(KiotVietUpdateOrderRequest request)
        {            
            await _publishEndpoint.Publish(request);
            return Ok(request);
        }
       
        //
        public Task<ApiResponse> UpdateInvoince(KiotVietUpdateInvoiceRequest request)
        {
            throw new NotImplementedException();
        }

        //
        public Task<ApiResponse> UpdateStock(KiotVietUpdateStockRequest request)
        {
            throw new NotImplementedException();
        }

        public Task<ApiResponse> DeleteStock(int[] removeId)
        {
            throw new NotImplementedException();
        }
        //
        public Task<ApiResponse> DeletePriceBook(int[] removeId)
        {
            throw new NotImplementedException();
        }

        public Task<ApiResponse> UpdatePriceBook(KiotVietUpdatePriceBookRequest request)
        {
            throw new NotImplementedException();
        }
        //

        public async Task<ApiResponse> UpdateBranch(KiotVietUpdateBranch request)
        {
            await _publishEndpoint.Publish(request);
            return Ok(request);
        }
        public Task<ApiResponse> DeleteBranch(int[] removeId)
        {
            throw new NotImplementedException();
        }

        public Task<ApiResponse> UpdateCategory(KiotVietUpdateCategory request)
        {
            throw new NotImplementedException();
        }
        public Task<ApiResponse> DeleteCategory(int[] removeId)
        {
            throw new NotImplementedException();
        }

        public Task<ApiResponse> UpdatePriceBookDetail(KiotVietUpdatePriceBookDetailRequest request)
        {
            throw new NotImplementedException();
        }

        public Task<ApiResponse> DeletePriceBookDetail(KiotVietDeletePriceBookDetail request)
        {
            throw new NotImplementedException();
        }
    }
}
