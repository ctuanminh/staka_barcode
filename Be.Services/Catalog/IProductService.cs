using Be.Common.Dtos.Product;
using Be.Common.Product.Response;
using Be.Common.Responses;
using Be.Core.Entities;

namespace Be.Services.Catalog
{
	public interface IProductService
	{
		Task<List<Product>> GetProducts();
        Task<List<Product>> GetProductsByBranchId(long branchId);

        Task<List<ProductCodeBarCode>> GetProductCodeBarCode();
        Task<ApiResponse> InsertProduct(ProductCreateRequest request);
		Task<ApiResponse> UpdateProduct(ProductUpdateRequest request);
		Task<ApiResponse> DeleteProduct(Guid Id);
		Task<ApiResponse> SyncProduct(SearchProductRequestKiot searchProductRequestKiot);
		Task<Product> GetProductById(long Id);
        Task<List<ProductCodeBarCode>> SynAndGetProductCodeBarCode(List<string> productCodes, int branchId);
    }
}
