using Be.Common.Dtos.CashFlow;
using Be.Common.Request;
using Be.Common.Responses;
using Be.Common.utils;
using Be.Core.Entities;
using Be.Data.Repository;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using OfficeOpenXml;
using System.Buffers.Text;

namespace Be.Services.CashFlow
{
    public class CashFlowServiceImp : ServiceResponse, ICashFlowService
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _config;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IRepository _repository;
        private readonly string _baseUrl = "https://public.kiotapi.com/cashflow";
        public CashFlowServiceImp(IHttpClientFactory httpClientFactory, IConfiguration config, IRepository repository)
        {
            _httpClientFactory = httpClientFactory;
            _config = config;
            _httpClient = _httpClientFactory.CreateClient();
            _repository = repository;
        }
        public async Task<ApiResponse> GetAllCashFlow(CashFlowRequest request)
        {
            request.IncludeUser = true; //Lấy thông tin người tạo
            request.IncludeBranch = true; // lấy thông tin chi nhánh
            request.IncludeAccount = true; // Bao gồm thông tin tài khoản

            request.PageSize = request.PageSize <= 0 ? 10 : request.PageSize;
            request.CurrentItem = request.CurrentItem <= 0 ? 0 : (request.CurrentItem - 1) * request.PageSize;

            var (success, content) = await KiotVietApiHelper.CallApiAsync(_httpClient, _config, _baseUrl, request);

            if (!success || content == null)
            {
                return BadRequest("B", "Có lỗi trong quá trình lấy dữ liệu Kiotviet");
            }
            var cashFlowApi = JsonConvert.DeserializeObject<CashFlowApiReponse>(content);           
            
            var pageResult = new PagedResult<CashFlowReponse>
            {
                TotalCount = cashFlowApi.Total,
                PageIndex = cashFlowApi.CurrentItem,
                PageSize = cashFlowApi.PageSize,
                Items = cashFlowApi.Data
            };
            return Ok(pageResult);
        }
        public Task<ApiResponse> GetCashFlowByIdAsync(int id)
        {
            throw new NotImplementedException();
        }
        public Task<IEnumerable<ApiResponse>> GetCashFlowsAsync()
        {
            throw new NotImplementedException();
        }
        public Task<ApiResponse> CreateCashFlowAsync(Cashflow cashFlow)
        {
            throw new NotImplementedException();
        }
        public Task<ApiResponse> UpdateCashFlowAsync(Cashflow cashFlow)
        {
            throw new NotImplementedException();
        }
        public Task<bool> DeleteCashFlowAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<byte[]> ExportMisaTemplate(SearchCashflowRequest request, string templatePath)
        {
            request.IncludeUser = true; //Lấy thông tin người tạo
            request.IncludeBranch = true; // lấy thông tin chi nhánh
            request.IncludeAccount = true; // Bao gồm thông tin tài khoản

            var numRequest = (int)Math.Ceiling((double)request.PageSize / 100);
            
            request.PageSize = request.PageSize <= 0 ? 10 : request.PageSize;
            
            var allCashflow = new List<CashFlowReponse>();
            int currentPage = 1, pageSize = 200, totalPages;
            request.PageSize = 200;
            do
            {
                request.CurrentItem = (currentPage - 1) * pageSize;
                var (Success, Content) = await KiotVietApiHelper.CallApiAsync(_httpClient, _config, _baseUrl, request);
                if (!Success || Content == null)
                {
                    return null;
                }
                var cashFlowApi = JsonConvert.DeserializeObject<CashFlowApiReponse>(Content);
                allCashflow.AddRange(cashFlowApi.Data);
                totalPages = (int)Math.Ceiling((double)cashFlowApi.Total / pageSize);
                currentPage++;
            }
            while (currentPage <= totalPages);

            var file = await File.ReadAllBytesAsync(templatePath);
            using var stream = new MemoryStream(file);
            using var package = new ExcelPackage(stream);
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            ExcelWorksheet worksheet = package.Workbook.Worksheets[0];
            worksheet.Name = $"SO QUY";
            int startRow = 9;

            foreach (var item in allCashflow)
            {
                int currentRow = startRow;
                worksheet.InsertRow(startRow, 1, 9); // Chèn một hàng mới vào vị trí bắt đầu
                worksheet.Cells[currentRow, 1].Value = item.TransDate.ToString("dd/MM/yyyy"); // Ngày chứng từ
                worksheet.Cells[currentRow, 2].Value = item.TransDate.ToString("dd/MM/yyyy"); // Ngày hạch toán
                worksheet.Cells[currentRow, 3].Value = item.Code; // Số chứng từ
                worksheet.Cells[currentRow, 4].Value = item.Description; // Diễn giải
                worksheet.Cells[currentRow, 5].Value = "VNĐ"; // Loại tiền
                worksheet.Cells[currentRow, 6].Value = ""; // Tỷ giá
                worksheet.Cells[currentRow, 7].Value = item.Description; // Diễn giải (Hạch toán)
                worksheet.Cells[currentRow, 8].Value = "131QB"; // TK Nợ
                worksheet.Cells[currentRow, 9].Value = "711QB"; // TK Có
                worksheet.Cells[currentRow, 10].Value = item.Amount; // Số tiền
                worksheet.Cells[currentRow, 11].Value = ""; // Số tiền quy đổi  
                worksheet.Cells[currentRow, 12].Value = item.PartnerName; // Mã đối tượng Nợ
                worksheet.Cells[currentRow, 13].Value = ""; // Mã đối tượng Có
                worksheet.Cells[currentRow, 14].Value = "không"; // Hạch toán gộp nhiều hóa đơn                
            }
            return package.GetAsByteArray();
        }
    }
}
