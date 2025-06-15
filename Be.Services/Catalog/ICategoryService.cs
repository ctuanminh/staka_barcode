using Be.Common.Dtos.Category;
using Be.Common.Responses;

namespace Be.Services.Catalog
{
	public interface ICategoryService
	{
		Task<ApiResponse> GetAllCategory(SearchCategoryRequest request);
		Task<ApiResponse> GetCategoryById(Guid id);
		Task<ApiResponse> AddCategory(CategoryDto request);
	}
}
