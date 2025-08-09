using Be.Common.Dtos.Product;
using Be.Common.Order.Response;
using Be.Common.Product.Response;
using Be.Common.Responses;
using Be.Core.Entities;
using Be.Data.Repository;
using Be.Services.KiotViet;
using Be.Services.System;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace Be.Services.Catalog
{
	public class ProductServiceImp(
        IRepository repository,
        IKiotVietService kiotVietService,
        ISystemService systemService)
        : ServiceResponse, IProductService
    {
        public async Task<ApiResponse> InsertProduct(ProductCreateRequest request)
        {
            const string rootPath = "";
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
            request.includeInventory = true;
            var productList = new List<ProductDto>();
            var currentPage = 1;
            int totalPages;
            const int pageSize = 200;
            do
            {
                request.CurrentItem = (currentPage - 1) * request.PageSize;
                var (success, content) = await kiotVietService.CallApiAsync(baseUrl, request);
                if (!success || content == null) return null;
                var productApiResponse = JsonConvert.DeserializeObject<ProductApiResponse>(content);
                totalPages = (int)Math.Ceiling((double)productApiResponse.Total / pageSize);
                productList.AddRange(productApiResponse.Data);
                currentPage++;
            } while (currentPage <= totalPages);

            var products = new List<Product>();

            foreach (var item in productList)
            {            
                var productExist = repository.GetQueryable<Product>()
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
                        BasePrice = item.BasePrice,
                    };
                    products.Add(product);
                }
                else
                {
                    productExist.Name = item.Name;
                    productExist.Unit = item.Unit;
                    productExist.BarCode = string.IsNullOrEmpty(item.BarCode) ? item.Code : item.BarCode;
                    productExist.IsActive = true;
                    productExist.BasePrice = item.BasePrice;
                    await repository.UpdateAsync(productExist);                    
                    await repository.SaveChangeAsync();
                }
            }
            await repository.AddRangeAsync<Product, long>(products);
            await repository.SaveChangeAsync();
            return Ok(products);
        }

        public async Task<List<Product>> GetProducts(long branchId)
        {
            var query = await repository.GetQueryable<Product>()
                .Select(p => new Product()
                {
                    Id = p.Id,
                    Code = p.Code,
                    BarCode = p.BarCode,
                    Name = p.Name,
                    Unit = p.Unit,
                })
                .ToListAsync();
            return query;
        }

        public async Task<List<ProductCodeBarCode>> GetProductCodeBarCode()
        {
            var products = await repository.GetQueryable<Product>()
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
            var product = await repository.FindAsync<Product, long>(x => x.Id == Id);
            return product;
        }

        public async Task<List<ProductCodeBarCode>> SynAndGetProductCodeBarCode(List<string> productCodes, int branchId)
        {
            if (productCodes.Count >0)
            {
                //Lấy code đã có trong db
                var existCodes = await repository.GetQueryable<Product>()
                    .Where(p => productCodes.Contains(p.Code))
                    .Select(p => p.Code)
                    .ToListAsync();
                //Lọc ra code chưa có
                var missingCodes = productCodes.Except(existCodes).ToList();

                var products = new List<Product>();

                //lấy sản phẩm code còn thiếu add vào db.
                foreach (var code in missingCodes)
                {
                    var productUrl = $"https://public.kiotapi.com/products/code/{code}";
                    var (success, content) = await kiotVietService.CallApiAsync(productUrl, (string)null);
                    await systemService.AddRequest(new RequestEntity()
                    {
                        Module = "SyncProduct",
                        Url = productUrl,
                        IsSuccess = success,
                        BranchId = branchId
                    });
                    if (!success || string.IsNullOrWhiteSpace(content)) continue;
                    var productKiotDto = JsonConvert.DeserializeObject<ProductDto>(content);
                    var product = new Product()
                    {
                        Id = productKiotDto.Id,
                        Code = productKiotDto.Code,
                        BarCode = string.IsNullOrEmpty(productKiotDto.BarCode)
                            ? productKiotDto.Code
                            : productKiotDto.BarCode,
                        Name = productKiotDto.Name,
                        Unit = productKiotDto.Unit,
                        IsActive = true,
                    };
                    products.Add(product);
                }

                if (products.Any())
                {
                    await repository.AddRangeAsync<Product, long>(products);
                    await repository.SaveChangeAsync();
                }
            }

            var productCodeBarCodes = await repository.GetQueryable<Product>()
                .Where(p => p.IsActive)
                .Select(p => new ProductCodeBarCode
                {
                    Code = p.Code,
                    BarCode = p.BarCode,
                    Unit= p.Unit

                }).ToListAsync();
            return productCodeBarCodes;
        }

        public async Task<Dictionary<string, string>> GetProductCodeDictionary()
        {
            var productDictionary = await repository.GetQueryable<Product>()
                .Where(p => p.IsActive)
                .ToDictionaryAsync(d => d.Code, d => d.BarCode);
            return productDictionary;
        }

    }
}
