using Be.Common.Purchase_Order.Dto;
using Be.Common.Purchase_Order.Request;
using Be.Common.Purchase_Order.Response;
using Be.Common.Responses;
using Be.Common.utils;
using Be.Core.Entities;
using Be.Data.Repository;
using Be.Services.Catalog;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using OfficeOpenXml;
using System.IO;
namespace Be.Services.PurchaseOrder
{
    public partial class PurchaseOrderServiceImp : ServiceResponse, IPurchaseOrderService
    {
        private readonly string _baseUrl = "https://public.kiotapi.com/purchaseorders";
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _config;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IRepository _repository;
        private readonly IProductService _productService;
        public PurchaseOrderServiceImp(IHttpClientFactory httpClientFactory, IRepository repository, IConfiguration config, IProductService productService)
        {
            _config = config;
            _httpClientFactory = httpClientFactory;
            _httpClient = _httpClientFactory.CreateClient();
            _repository = repository;
            _productService = productService;
        }
        public Task<ApiResponse> CreatePurchaseOrder(PurchaseOrderDto purchaseOrderDto)
        {
            throw new NotImplementedException();
        }

        public Task<ApiResponse> DeletePurchaseOrder(Guid id)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Export purchase orders to Misa format using a template file.
        /// </summary>
        /// <param name="request"></param>
        /// <param name="templatePath"></param>
        /// <returns></returns>
        public async Task<byte[]> ExportPurchaseOrderMisa(SearchPurchaseOrderRequest request, string templatePath)
        {
            request.IncludePayment = true;
            request.PageSize = 200;
            var purchaseOrderList = new List<PurchaseOrderReponse>();
            int totalPages = 1, currentPage = 1, pageSize = 200;            
            do
            {
                request.CurrentItem = (currentPage - 1) * pageSize;
                var (Success, Content) = await KiotVietApiHelper.CallApiAsync(_httpClient, _config, _baseUrl, request);
                if (Success && Content != null)
                {
                    var purchaseOrderPagedData = JsonConvert.DeserializeObject<PurchaseOrderPagedData>(Content);
                    if (purchaseOrderPagedData?.Data != null)
                        purchaseOrderList.AddRange(purchaseOrderPagedData.Data);                    
                    if (currentPage == 1 && purchaseOrderPagedData.Total > pageSize)
                        totalPages = (int)Math.Ceiling((double)purchaseOrderPagedData.Total / pageSize);
                }
                currentPage++;
            } while (currentPage <= totalPages);

            // Lấy thông tin sản phẩm ĐVT            
            var productIds = purchaseOrderList
                .SelectMany(x => x.PurchaseOrderDetails)
                .Select(d => d.ProductId)
                .Distinct()
                .ToList();

            // Truy vấn EF với productIds
            var productUnitDict = (await _repository.GetQueryable<Product, long>()
                .Where(p => productIds.Contains(p.Id))
                .Select(p => new { p.Id, p.Unit })
                .ToListAsync())
                .ToDictionary(p => p.Id, p => p.Unit);

            var file = await File.ReadAllBytesAsync(templatePath);
            using var stream = new MemoryStream(file);
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            using var package = new ExcelPackage(stream);
            var worksheet = package.Workbook.Worksheets[0];
            worksheet.Name = "Mua hang trong nuoc"; 
            int startRow = 9;
            foreach (var purchaseOrder in purchaseOrderList)
            {
                foreach (var purchaseOrderDetail in purchaseOrder.PurchaseOrderDetails)
                {
                    int currentRow = startRow;
                    worksheet.InsertRow(startRow, 1, 9); // Chèn một hàng mới vào vị trí bắt đầu
                    worksheet.Cells[currentRow, 1].Value = "Mua hàng trong nước nhập kho";
                    worksheet.Cells[currentRow, 2].Value = "Chưa thanh toán";
                    worksheet.Cells[currentRow, 3].Value = "";
                    worksheet.Cells[currentRow, 4].Value = purchaseOrder.PurchaseDate.ToString("dd/MM/yyyy"); //Ngày hoạch toán
                    worksheet.Cells[currentRow, 5].Value = purchaseOrder.PurchaseDate.ToString("dd/MM/yyyy"); // Ngày chứng từ
                    worksheet.Cells[currentRow, 6].Value = purchaseOrder.Code; //Số phiếu
                    worksheet.Cells[currentRow, 7].Value = "";
                    worksheet.Cells[currentRow, 8].Value = ""; // Số hoá đơn
                    worksheet.Cells[currentRow, 9].Value = ""; // Ngày hoá đơn
                    worksheet.Cells[currentRow, 10].Value = purchaseOrder.SupplierCode; // Mã Nhà cung cấp
                    worksheet.Cells[currentRow, 11].Value = purchaseOrder.SupplierName; // Tên Nhà cung cấp
                    worksheet.Cells[currentRow, 12].Value = ""; // Địa chỉ
                    worksheet.Cells[currentRow, 13].Value = ""; // Mã số thuế
                    worksheet.Cells[currentRow, 14].Value = ""; // Người giao hàng
                    worksheet.Cells[currentRow, 15].Value = ""; // Số tài khoản chi
                    worksheet.Cells[currentRow, 16].Value = ""; // Tên ngân hàng
                    worksheet.Cells[currentRow, 17].Value = $"Mua hàng của {purchaseOrder.SupplierName}"; // Diễn giải
                    worksheet.Cells[currentRow, 18].Value = ""; // Loại tiền
                    worksheet.Cells[currentRow, 19].Value = ""; // Tỉ giá.                    
                    worksheet.Cells[currentRow, 20].Value = purchaseOrderDetail.ProductCode; // Mã hàng hóa
                    worksheet.Cells[currentRow, 21].Value = purchaseOrderDetail.ProductName; // Tên hàng hóa
                    worksheet.Cells[currentRow, 22].Value = purchaseOrder.BranchId; // Mã kho
                    worksheet.Cells[currentRow, 23].Value = "1561"; // Tài khoản kho/Tk chi phí
                    worksheet.Cells[currentRow, 24].Value = "331"; // Tài khoản công nợ
                    worksheet.Cells[currentRow, 25].Value = productUnitDict.TryGetValue(purchaseOrderDetail.ProductId, out var unit)? unit : ""; // Đơn vị tính
                    worksheet.Cells[currentRow, 26].Value = purchaseOrderDetail.Quantity; // Số lượng
                    worksheet.Cells[currentRow, 27].Value = purchaseOrderDetail.Price; // Đơn giá
                    worksheet.Cells[currentRow, 28].Value = purchaseOrderDetail.Price * purchaseOrderDetail.Quantity; // Thành tiền
                }
            }
            return package.GetAsByteArray();
        }

        public async Task<ApiResponse> GetAllPurchaseOrders(SearchPurchaseOrderRequest request)
        {
            request.IncludePayment = true;
            request.OrderBy = "PurchaseDate";
            request.OrderDirection = "Desc";
            request.CurrentItem = (request.CurrentItem - 1) * request.PageSize;
            var (Success, Content) = await KiotVietApiHelper.CallApiAsync(_httpClient, _config, _baseUrl, request);
            if (!Success || Content == null)
            {
                return BadRequest("B", "Failed to retrieve purchase orders from KiotViet API.");
            }
            var purchaseOrderPagedData = JsonConvert.DeserializeObject<PurchaseOrderPagedData>(Content);
            var pageResult = new PagedResult<PurchaseOrderReponse>()
            {
                PageSize = purchaseOrderPagedData.PageSize,
                PageIndex = purchaseOrderPagedData.CurrentItem,
                Items = purchaseOrderPagedData.Data,
                TotalCount = purchaseOrderPagedData.Total
            };
            return Ok(pageResult);
        }

        public Task<ApiResponse> GetPurchaseOrderById(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<ApiResponse> ImportPurchaseOrderMisa(Stream fileStream, string fileName)
        {
            throw new NotImplementedException();
        }

        public Task<ApiResponse> UpdatePurchaseOrder(Guid id, PurchaseOrderDto purchaseOrderDto)
        {
            throw new NotImplementedException();
        }
    }
}
