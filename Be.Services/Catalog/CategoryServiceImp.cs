using Be.Common.Dtos.Category;
using Be.Common.Responses;
using Be.Core.Entities;
using Be.Data.Repository;
using Microsoft.EntityFrameworkCore;

namespace Be.Services.Catalog
{
	public class CategoryServiceImp : ServiceResponse, ICategoryService
	{
		private readonly IRepository _repository;

		public CategoryServiceImp(IRepository repository)
		{
			_repository = repository;
		}

		public async Task<ApiResponse> AddCategory(CategoryDto request)
		{
			var categoryExist = _repository.GetQueryable<Category>().Where(x => x.Name.Equals(request.Name)).FirstOrDefault();
			if (categoryExist != null)
				return BadRequest("B", "Category aready exist!");
			var category = new Category
			{
				Name = request.Name,
				Description = request.Description
			};
			await _repository.AddAsync(category);
			await _repository.SaveChangeAsync();
			return Ok(category);
		}

		public async Task<ApiResponse> GetAllCategory(SearchCategoryRequest request)
		{
			var query = _repository.GetQueryable<Category>();
			var page = await _repository.FindPagedAsync(query, request.PageIndex, request.PageSize);
			var listCategory = new List<CategoryDto>();
			foreach (var item in page.Items)
			{
				var category = new CategoryDto()
				{
					Id = item.Id,
					Name = item.Name,
					Description = item.Description,
					CreatedAt = item.CreatedAt
				};
                listCategory.Add(category);
			}
			var pageResult = new PagedResult<CategoryDto>()
			{
				PageIndex = page.PageIndex,
				PageSize = page.PageSize,
				Items = listCategory,
				TotalCount = page.TotalCount
			};
			return Ok(pageResult);
		}

		public async Task<ApiResponse> GetCategoryById(Guid id)
		{
			var category = await _repository.FindAsync<Category>(id);
			if(category != null)
			{
				return Ok(category);
			}
			return BadRequest("B", "Category type not found!");
        }
	}
}
