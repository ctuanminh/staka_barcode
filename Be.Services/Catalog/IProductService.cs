using Be.Common.Dtos.Product;
using Be.Common.Product.Response;
using Be.Common.Request;
using Be.Common.Responses;
using Be.Core.Entities;

namespace Be.Services.Catalog
{
	public interface IProductService
	{
		Task<List<ProductCodeBarCode>> GetProductCodeBarCode();
        Task<ApiResponse> InsertProduct(ProductCreateRequest request);
		Task<ApiResponse> UpdateProduct(ProductUpdateRequest request);
		Task<ApiResponse> DeleteProduct(Guid Id);
		Task<ApiResponse> SyncProduct(SearchProductRequestKiot searchProductRequestKiot);
		Task<Product> GetProductById(long Id);
    }
}
