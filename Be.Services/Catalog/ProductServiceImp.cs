using Be.Common.Dtos.Category;
using Be.Common.Dtos.Product;
using Be.Common.Product.Response;
using Be.Common.Request;
using Be.Common.Responses;
using Be.Common.utils;
using Be.Core.Entities;
using Be.Data.Repository;
using Be.Services.KiotViet;
using DocumentFormat.OpenXml.Wordprocessing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.Diagnostics.CodeAnalysis;
using System.Net.Http.Headers;
using System.Text.Json;

namespace Be.Services.Catalog
{
	public class ProductServiceImp : ServiceResponse, IProductService
	{
		private readonly IRepository _repository;
        private readonly IKiotVietService _kiotVietService;

        public ProductServiceImp(IRepository repository, IKiotVietService kiotVietService)
        {
            _repository = repository;
            _kiotVietService = kiotVietService;
        }

		public async Task<ApiResponse> InsertProduct(ProductCreateRequest request)
        {
            var rootPath = "";
			var fullPath = Path.Combine(rootPath, "wwwroot", "images", "products");
            
            if (!Directory.Exists(fullPath))
			{
                Directory.CreateDirectory(fullPath);
            }
			var saveFiles = new List<string>();
            foreach (var file in request.ItemImages)
			{
                var fileName = $"{Guid.NewGuid()}_{Path.GetFileName(file.FileName)}";
                var filePath = Path.Combine(fullPath, fileName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }
                saveFiles.Add(fileName);
            }
            var product = new Product()
            {
                Name = request.Name,
                Description = request.Description,
                //Content = request.Content,
                //Price = request.Price,
                //ItemImages = CommonHelper.SerializeObject(saveFiles),
                //CategoryId = request.CategoryId
            };
            //trongcv comment
            //await _repository.AddAsync<long>(product);
            //await _repository.SaveChangeAsync();
            return Ok(product);

        }

		public Task<ApiResponse> UpdateProduct(ProductUpdateRequest request)
		{
			throw new NotImplementedException();
		}

		public Task<ApiResponse> DeleteProduct(Guid Id)
		{
			throw new NotImplementedException();
		}

        public async Task<ApiResponse> SyncProduct(SearchProductRequestKiot request)
        {
            const string baseUrl = "https://public.kiotapi.com/products";
            request.PageSize = request.PageSize != 0 ? request.PageSize : 200;

            var productList = new List<ProductDto>();
            var currentPage = 1;
            int totalPages;
            const int pageSize = 200;
            do
            {
                request.CurrentItem = (currentPage - 1) * request.PageSize;
                var (success, content) = await _kiotVietService.CallApiAsync(baseUrl, request);
                if (!success || content == null) return null;
                var productApiResponse = JsonConvert.DeserializeObject<ProductApiResponse>(content);
                totalPages = (int)Math.Ceiling((double)productApiResponse.Total / pageSize);
                productList.AddRange(productApiResponse.Data);
                currentPage++;
            } while (currentPage <= totalPages);

            var products = new List<Product>();

            foreach (var item in productList)
            {            
                var productExist = _repository.GetQueryable<Product>()
                    .FirstOrDefault(x => x.Id == item.Id);
                if (productExist is null)
                {
                    var product = new Product()
                    {
                        Id = item.Id,
                        Code = item.Code,
                        BarCode = string.IsNullOrEmpty(item.BarCode)? item.Code : item.BarCode,
                        Name = item.Name,
                        Unit = item.Unit,
                        IsActive = true,
                    };
                    products.Add(product);
                }
                else
                {
                    productExist.Name = item.Name;
                    productExist.Unit = item.Unit;
                    productExist.BarCode = string.IsNullOrEmpty(item.BarCode) ? item.Code : item.BarCode;
                    productExist.IsActive = true;
                    await _repository.UpdateAsync(productExist);                    
                    await _repository.SaveChangeAsync();
                }
            }
            await _repository.AddRangeAsync<Product, long>(products);
            await _repository.SaveChangeAsync();
            return Ok(products);
        }

        public async Task<List<ProductCodeBarCode>> GetProductCodeBarCode()
        {
            var products = await _repository.GetQueryable<Product>()
                .Where(p => p.IsActive)
                .Select(p => new ProductCodeBarCode
                {
                    Code = p.Code,
                    BarCode = p.BarCode
                }).ToListAsync();
            return products;
        }

        Task<ApiResponse> IProductService.InsertProduct(ProductCreateRequest request)
        {
            throw new NotImplementedException();
        }

        Task<ApiResponse> IProductService.UpdateProduct(ProductUpdateRequest request)
        {
            throw new NotImplementedException();
        }

        Task<ApiResponse> IProductService.DeleteProduct(Guid Id)
        {
            throw new NotImplementedException();
        }

        public async Task<Product> GetProductById(long Id)
        {
            var product = await _repository.FindAsync<Product, long>(x => x.Id == Id);
            return product;
        }

        public async Task<Dictionary<string, string>> GetProductCodeDictionary()
        {
            var productDictionary = await _repository.GetQueryable<Product>()
                .Where(p => p.IsActive)
                .ToDictionaryAsync(d => d.Code, d => d.BarCode);
            return productDictionary;
        }
    }
}
