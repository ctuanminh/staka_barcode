using Be.Common.Branch.Response;
using Be.Common.customer;
using Be.Common.customer.Request;
using Be.Common.customer.Response;
using Be.Common.Dtos.Invoice;
using Be.Common.Responses;
using Be.Common.utils;
using Be.Core.Entities;
using Be.Data.Repository;
using Be.Services.Catalog;
using MassTransit.Initializers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using OfficeOpenXml;
using System.Collections.Concurrent;
using System.IO.Packaging;
using System.Net.Http.Headers;
using System.Text.Json;

namespace Be.Services.Order
{
    public class OrderServiceImp : ServiceResponse, IOrderService
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _config;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IRepository _repository;
        private readonly IProductService _productService;

        public OrderServiceImp(IConfiguration config, IHttpClientFactory httpClientFactory, IRepository repository, IProductService productService)
        {
            _config = config;
            _httpClientFactory = httpClientFactory;
            _httpClient = _httpClientFactory.CreateClient(); // Tạo HttpClient từ factory
            _repository = repository;
            _productService = productService;
        }

        public async Task<ApiResponse> GetAllCustomer(SearchCustomerRequest request)
        {
            var isHeaderReady = await PrepareAuthorizedHeadersAsync();
            if (!isHeaderReady)
            {
                return BadRequest("B", "Token is not valid");
            }
            request.PageSize = request.PageSize <= 0 ? 10 : request.PageSize;
            request.currentItem = request.currentItem <= 0 ? 0 : (request.currentItem - 1) * request.PageSize;

            var baseUrl = "https://public.kiotapi.com/customers";
            var url = QueryStringHelper.BuildQueryString(request, baseUrl);
            var response = await _httpClient.GetAsync(url);
            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                return BadRequest("B", error);
            }

            var responseData = await response.Content.ReadAsStringAsync();
            var customerApiResponse = JsonConvert.DeserializeObject<CustomerPagedResponse>(responseData);            
            var pageResult = new PagedResult<CustomerResponse>()
            {
                PageSize = customerApiResponse.PageSize,
                PageIndex = customerApiResponse.CurrentItem,
                Items = customerApiResponse.Data,
                TotalCount = customerApiResponse.Total
            };
            return Ok(pageResult);
        }

        private async Task<CustomerResponse> GetCustomerById(int customerId)
        {
            var url = $"https://public.kiotapi.com/customers/{customerId}";
            var response = await _httpClient.GetAsync(url);
            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                throw new Exception($"Error fetching customer {customerId}: {error}");
            }
            var responseData = await response.Content.ReadAsStringAsync();
            var customerDetails = JsonConvert.DeserializeObject<CustomerResponse>(responseData);            

            return customerDetails;
        }

        private async Task<BranchResponse> GetBranchDetailsAsync(int branchId)
        {
            var isHeaderReady = await PrepareAuthorizedHeadersAsync();
            if (!isHeaderReady)
            {
                throw new Exception($"Error fetching branch");
            }
            var url = $"https://public.kiotapi.com/branches/{branchId}";
            var response = await _httpClient.GetAsync(url);
            if (!response.IsSuccessStatusCode)
            {
                return null;
            }

            var responseData = await response.Content.ReadAsStringAsync();
            var branchDetails = JsonConvert.DeserializeObject<BranchResponse>(responseData);

            return branchDetails;
        }

        private async Task<bool> PrepareAuthorizedHeadersAsync()
        {
            var token = await GetAccessTokenAsync();
            if (string.IsNullOrEmpty(token))
            {
                return false;
            }
            _httpClient.DefaultRequestHeaders.Clear();
            _httpClient.DefaultRequestHeaders.Add("Retailer", _config["KiotViet:Retailer"]);
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            return true;
        }

        private async Task<string> GetAccessTokenAsync()
        {
            var tokenUrl = _config["KiotViet:TokenUrl"];
            var tokenRequest = new FormUrlEncodedContent(
            [
                new KeyValuePair<string, string>("client_id", _config["KiotViet:ClientId"]),
                new KeyValuePair<string, string>("client_secret", _config["KiotViet:ClientSecret"]),
                new KeyValuePair<string, string>("grant_type", "client_credentials"),
                new KeyValuePair<string, string>("scope", "PublicApi.Access")
            ]);
            var response = await _httpClient.PostAsync(tokenUrl, tokenRequest);
            if (!response.IsSuccessStatusCode)
            {
                return null;
            }
            else
            {
                var tokenData = await response.Content.ReadAsStringAsync();
                var tokenJson = JsonDocument.Parse(tokenData);
                return tokenJson.RootElement.GetProperty("access_token").GetString();
            }
        }

        public async Task<ApiResponse> GetAllInvoice(SearchInvoiceRequest request, string templatePath)
        {
            string exportFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\Exports");
            string timestamp = DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss");

            string destFilename = Path.Combine(exportFolder, $"misa_export_{timestamp}.xlsx");

            File.Copy(templatePath, destFilename, true);

            var isHeaderReady = await PrepareAuthorizedHeadersAsync();
            if (!isHeaderReady)
            {
                return BadRequest("Token", "Token is not valid");
            }
            request.PageSize = request.PageSize <= 0 ? 10 : request.PageSize;
            request.CurrentItem = request.CurrentItem <= 0 ? 0 : (request.CurrentItem - 1) * request.PageSize;

            var baseUrl = "https://public.kiotapi.com/invoices";
            var url = QueryStringHelper.BuildQueryString(request, baseUrl);
            var response = await _httpClient.GetAsync(url);
            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                return BadRequest("Error", error);
            }
            var responseData = await response.Content.ReadAsStringAsync();
            var invoiceApiResponse = JsonConvert.DeserializeObject<InvoiceApiResponse>(responseData);

            // Chuẩn bị dữ liệu từ KiotViet
            var misaRows = new List<Dictionary<string, object>>();
            foreach (var invoice in invoiceApiResponse.Data)
            {
                //string date = invoice.PurchaseDate;
                string code = invoice.Code;
                var customer = invoice.CustomerName;
                string customerName = invoice.CustomerName ?? "";
                string customerCode = invoice.CustomerCode ?? "";

                foreach (var item in invoice.InvoiceDetails)
                {
                    var product = await _httpClient.GetAsync("https://public.kiotapi.com/products/code/" + item.ProductCode);
                    var responseDataProduct = await product.Content.ReadAsStringAsync();
                    var dataProduct = JsonConvert.DeserializeObject<ProductInvoice>(responseDataProduct);
                    misaRows.Add(new Dictionary<string, object>
                    {
                        { "Hình thức bán hàng", "Bán hàng hóa trong nước" },
                        { "Phương thức thanh toán", "Chưa thu tiền" },
                        { "Kiểm phiếu xuất kho", "Có" },
                        { "Lập kèm hóa đơn", "" },
                        { "Đã lập hóa đơn", "" },
                        { "Ngày hạch toán (*)", invoice.PurchaseDate.ToString("dd/MM/yyyy") },
                        { "Ngày chứng từ (*)", invoice.PurchaseDate.ToString("dd/MM/yyyy") },
                        { "Số chứng từ (*)", code },
                        { "Số phiếu xuất", $"{code}.1" },
                        { "Mẫu số HĐ", "" },
                        { "Ký hiệu HĐ", "" },
                        { "Số hoá đơn", "" },
                        { "Ngày hoá đơn", "" },
                        { "Mã khách hàng", customerCode },
                        { "Tên khách hàng", customerName },
                        { "Địa chỉ", invoice.CustomerName },
                        { "Mã số thuế", ""},
                        { "Đơn vị giao đại lý", ""},
                        { "Người nộp", ""},
                        { "Nạp vào TK", ""},
                        { "Tên ngân hàng", ""},
                        { "Diễn giải/Lý do nộp", "Bán hàng"},
                        { "Lý do xuất", "Xuất kho bán hàng"},
                        { "Loại tiền", "VNĐ"},
                        { "Tỉ giá", "-"},
                        { "Mã hàng (*)", item.ProductCode },
                        { "Tên hàng", item.ProductName },
                        { "Là dòng ghi chú", "không"},
                        { "Hàng khuyến mại", "không"},
                        { "TK Tiền/Chi phí/Nợ (*)", "131QB"},
                        { "TK Doanh thu/Có (*)", "5111QB"},
                        { "ĐVT", dataProduct.unit},
                        { "Số lượng (*)", item.Quantity },
                        { "Đơn giá", item.Price },
                        { "Thành tiền", Convert.ToDecimal(item.Quantity * Convert.ToDouble( item.Price)) },
                        { "Thành tiền quy đổi", ""},
                        { "Tỷ lệ CK (%)", item.DiscountRatio},
                        { "Tiền chiết khấu", (item.DiscountRatio * Convert.ToDouble( item.Price))/100},
                        { "Tiền chiết khấu quy đổi", ""},
                        { "TK chiết khấu", "5211QB"},
                        { "Giá tính thuế XK", ""},
                        {"% thuế xuất khẩu", "" },
                        {"Tiền thuế xuất khẩu", "" },
                        {"TK thuế xuất khẩu", "" },
                        {"% thuế GTGT", "" },
                        {"% thuế suất KHAC", "" },
                        {"Tiền thuế GTGT", "" },
                        {"Tiền thuế GTGT quy đổi", "" },
                        {"TK thuế GTGT", "" },
                        {"HH không TH trên tờ khai thuế GTGT", "" },
                        {"Mã kho", "KQB" },
                        {"TK giá vốn", "632QB" },
                        {"TK Kho", "1561QB" },
                        {"Đơn giá vốn", "" },
                        {"Tiền vốn", "" },
                        {"Hàng hóa giữ hộ/bán hộ", "" },
                    });
                }
            }

            // Tạo file Excel và đổ dữ liệu vào file
            using (var workbook = new ClosedXML.Excel.XLWorkbook(templatePath))
            {
                var worksheet = workbook.Worksheet("Ban hang");

                // Xác định vị trí bắt đầu từ hàng 10
                int row = 9;
                foreach (var rowData in misaRows)
                {
                    int col = 1;
                    foreach (var keyValue in rowData)
                    {
                        var value = keyValue.Value;
                        var cell = worksheet.Cell(row, col);

                        switch (value)
                        {
                            case DateTime dt:
                                cell.Style.DateFormat.Format = "dd/MM/yyyy";
                                cell.Value = dt;
                                break;

                            case int i:
                                cell.Style.NumberFormat.Format = "#,##0.0";
                                cell.Value = i;
                                break;

                            case double d:
                                cell.Style.NumberFormat.Format = "#,##0.0";
                                cell.Value = d;
                                break;

                            case decimal dec:
                                cell.Style.NumberFormat.Format = "#,##0.0";
                                cell.Value = dec;
                                break;

                            default:
                                cell.Value = value?.ToString() ?? "";
                                break;
                        }

                        col++;
                    }
                    row++;
                }

                // Lưu lại file Excel
                workbook.SaveAs(destFilename);
            }

            return Ok(destFilename);
        }

        public async Task<ApiResponse> GetAllOrder(SearchInvoiceRequest request)
        {
            var isHeaderReady = await PrepareAuthorizedHeadersAsync();
            if (!isHeaderReady)
            {
                return BadRequest("Token", "Token is not valid");
            }
            request.PageSize = request.PageSize <= 0 ? 10 : request.PageSize;
            request.CurrentItem = request.CurrentItem <= 0 ? 0 : (request.CurrentItem - 1) * request.PageSize;

            var baseUrl = "https://public.kiotapi.com/invoices";
            var url = QueryStringHelper.BuildQueryString(request, baseUrl);
            var response = await _httpClient.GetAsync(url);
            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                return BadRequest("Error", error);
            }
            var responseData = await response.Content.ReadAsStringAsync();
            var invoiceApiResponse = JsonConvert.DeserializeObject<InvoiceApiResponse>(responseData);

            var pageResult = new PagedResult<InvoiceResponse>()
            {
                PageSize = invoiceApiResponse.PageSize,
                PageIndex = invoiceApiResponse.CurrentItem,
                Items = invoiceApiResponse.Data,
                TotalCount = invoiceApiResponse.Total
            };
            return Ok(pageResult);

        }

        public async Task<ApiResponse> ExportInvoiceMisaV2(SearchInvoiceRequest request, string templatePath)
        {
            string exportFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\Exports");
            string timestamp = DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss");

            string destFilename = Path.Combine(exportFolder, $"misa_export_{timestamp}.xlsx");

            File.Copy(templatePath, destFilename, true);

            var isHeaderReady = await PrepareAuthorizedHeadersAsync();
            if (!isHeaderReady)
            {
                return BadRequest("Token", "Token is not valid");
            }
            request.PageSize = request.PageSize <= 0 ? 10 : request.PageSize;
            request.CurrentItem = request.CurrentItem <= 0 ? 0 : (request.CurrentItem - 1) * request.PageSize;

            var baseUrl = "https://public.kiotapi.com/invoices";
            var url = QueryStringHelper.BuildQueryString(request, baseUrl);
            var response = await _httpClient.GetAsync(url);
            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                return BadRequest("Error", error);
            }
            var responseData = await response.Content.ReadAsStringAsync();
            var invoiceApiResponse = JsonConvert.DeserializeObject<InvoiceApiResponse>(responseData);


            // Chuẩn bị dữ liệu từ KiotViet
            var misaRows = new List<Dictionary<string, object>>();
            foreach (var invoice in invoiceApiResponse.Data)
            {
                //string date = invoice.PurchaseDate;
                string code = invoice.Code;
                var customer = invoice.CustomerName;
                string customerName = invoice.CustomerName ?? "";
                string customerCode = invoice.CustomerCode ?? "";

                foreach (var item in invoice.InvoiceDetails)
                {
                    var product = await _httpClient.GetAsync("https://public.kiotapi.com/products/code/" + item.ProductCode);
                    var responseDataProduct = await product.Content.ReadAsStringAsync();
                    var dataProduct = JsonConvert.DeserializeObject<ProductInvoice>(responseDataProduct);
                    misaRows.Add(new Dictionary<string, object>
                    {
                        { "Hình thức bán hàng", "Bán hàng hóa trong nước" },
                        { "Phương thức thanh toán", "Chưa thu tiền" },
                        { "Kiểm phiếu xuất kho", "Có" },
                        { "Lập kèm hóa đơn", "" },
                        { "Đã lập hóa đơn", "" },
                        { "Ngày hạch toán (*)", invoice.PurchaseDate.ToString("dd/MM/yyyy") },
                        { "Ngày chứng từ (*)", invoice.PurchaseDate.ToString("dd/MM/yyyy") },
                        { "Số chứng từ (*)", code },
                        { "Số phiếu xuất", $"{code}.1" },
                        { "Mẫu số HĐ", "" },
                        { "Ký hiệu HĐ", "" },
                        { "Số hoá đơn", "" },
                        { "Ngày hoá đơn", "" },
                        { "Mã khách hàng", customerCode },
                        { "Tên khách hàng", customerName },
                        { "Địa chỉ", invoice.CustomerName },
                        { "Mã số thuế", ""},
                        { "Đơn vị giao đại lý", ""},
                        { "Người nộp", ""},
                        { "Nạp vào TK", ""},
                        { "Tên ngân hàng", ""},
                        { "Diễn giải/Lý do nộp", "Bán hàng"},
                        { "Lý do xuất", "Xuất kho bán hàng"},
                        { "Loại tiền", "VNĐ"},
                        { "Tỉ giá", "-"},
                        { "Mã hàng (*)", item.ProductCode },
                        { "Tên hàng", item.ProductName },
                        { "Là dòng ghi chú", "không"},
                        { "Hàng khuyến mại", "không"},
                        { "TK Tiền/Chi phí/Nợ (*)", "131QB"},
                        { "TK Doanh thu/Có (*)", "5111QB"},
                        { "ĐVT", dataProduct.unit},
                        { "Số lượng (*)", item.Quantity },
                        { "Đơn giá", item.Price },
                        { "Thành tiền", Convert.ToDecimal(item.Quantity * Convert.ToDouble( item.Price)) },
                        { "Thành tiền quy đổi", ""},
                        { "Tỷ lệ CK (%)", item.DiscountRatio},
                        { "Tiền chiết khấu", (item.DiscountRatio * Convert.ToDouble( item.Price))/100},
                        { "Tiền chiết khấu quy đổi", ""},
                        { "TK chiết khấu", "5211QB"},
                        { "Giá tính thuế XK", ""},
                        {"% thuế xuất khẩu", "" },
                        {"Tiền thuế xuất khẩu", "" },
                        {"TK thuế xuất khẩu", "" },
                        {"% thuế GTGT", "" },
                        {"% thuế suất KHAC", "" },
                        {"Tiền thuế GTGT", "" },
                        {"Tiền thuế GTGT quy đổi", "" },
                        {"TK thuế GTGT", "" },
                        {"HH không TH trên tờ khai thuế GTGT", "" },
                        {"Mã kho", "KQB" },
                        {"TK giá vốn", "632QB" },
                        {"TK Kho", "1561QB" },
                        {"Đơn giá vốn", "" },
                        {"Tiền vốn", "" },
                        {"Hàng hóa giữ hộ/bán hộ", "" },
                    });
                }
            }

            // Tạo file Excel và đổ dữ liệu vào file
            using (var workbook = new ClosedXML.Excel.XLWorkbook(templatePath))
            {
                var worksheet = workbook.Worksheet("Ban hang");

                // Xác định vị trí bắt đầu từ hàng 10
                int row = 9;
                foreach (var rowData in misaRows)
                {
                    int col = 1;
                    foreach (var keyValue in rowData)
                    {
                        var value = keyValue.Value;
                        var cell = worksheet.Cell(row, col);

                        switch (value)
                        {
                            case DateTime dt:
                                cell.Style.DateFormat.Format = "dd/MM/yyyy";
                                cell.Value = dt;
                                break;

                            case int i:
                                cell.Style.NumberFormat.Format = "#,##0.0";
                                cell.Value = i;
                                break;

                            case double d:
                                cell.Style.NumberFormat.Format = "#,##0.0";
                                cell.Value = d;
                                break;

                            case decimal dec:
                                cell.Style.NumberFormat.Format = "#,##0.0";
                                cell.Value = dec;
                                break;

                            default:
                                cell.Value = value?.ToString() ?? "";
                                break;
                        }
                        col++;
                    }
                    row++;
                }

                // Lưu lại file Excel
                workbook.SaveAs(destFilename);
            }

            return Ok(destFilename);
        }        

        /// <summary>
        /// Misa: Mẫu bán hàng
        /// </summary>
        /// <param name="request"></param>
        /// <param name="templatePath"></param>
        /// <returns></returns>
        public async Task<Byte[]> ExportInvoiceMisa(SearchInvoiceRequest request, string templatePath)
        {
            //set mặc định hoá đơn mới nhất lên đầu:
            request.OrderBy = "createdDate";
            request.OrderDirection = "desc";
            request.IncludePayment = true;

            var baseUrl = "https://public.kiotapi.com/invoices";
            var allInvoices = new List<InvoiceResponse>();
            int totalPages = 1;
            int currentPage = 1;
            request.PageSize = 200;
            int pageSize = 200;
            do
            {
                request.CurrentItem = (currentPage - 1) * pageSize;
                var (Success, Content) = await KiotVietApiHelper.CallApiAsync(_httpClient, _config, baseUrl, request);
                if (Success && Content != null)
                {
                    var invoiceData = JsonConvert.DeserializeObject<InvoiceApiResponse>(Content);
                    allInvoices.AddRange(invoiceData.Data);
                    if (currentPage == 1 && invoiceData.Total > pageSize)
                        totalPages = (int)Math.Ceiling((double)invoiceData.Total / pageSize);
                }
                currentPage++;
            } while (currentPage <= totalPages);
                                        
            var productIds = allInvoices.AsEnumerable()
                .SelectMany(x => x.InvoiceDetails.Select(d => d.ProductId))
                .Distinct()
                .ToList(); 

            var productDicKeyUnit = (await _repository.GetQueryable<Product, long>()
                .Where(p => productIds.Contains(p.Id))
                .Select(p => new { p.Id, p.Unit })
                .ToListAsync())
                .ToDictionary(p => p.Id, p => p.Unit);
            var branch = await _repository.GetQueryable<Branch>()
                .Where(b => b.BranchId == request.BranchIds[0])
                .FirstOrDefaultAsync();

            var file = await File.ReadAllBytesAsync(templatePath);
            using var stream = new MemoryStream(file);
            using var package = new ExcelPackage(stream);
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            ExcelWorksheet worksheet = package.Workbook.Worksheets[0];
            worksheet.Name = $"BAN HANG {branch.BranchName}";
            int startRow = 9;
            foreach (var invoice in allInvoices)
            {
                string customerName = invoice.CustomerName ?? "";
                string customerCode = invoice.CustomerCode ?? "";
                foreach (var item in invoice.InvoiceDetails)
                {
                    int currentRow = startRow;
                    worksheet.InsertRow(startRow, 1, 9); // Chèn một hàng mới vào vị trí bắt đầu
                    worksheet.Cells[currentRow, 1].Value = "Bán hàng hóa trong nước";
                    worksheet.Cells[currentRow, 2].Value = "Chưa thu tiền";
                    worksheet.Cells[currentRow, 3].Value = "Có";
                    worksheet.Cells[currentRow, 4].Value = ""; // Lập kèm hóa đơn
                    worksheet.Cells[currentRow, 5].Value = ""; // Đã lập hóa đơn    
                    worksheet.Cells[currentRow, 6].Value = invoice.PurchaseDate.ToString("dd/MM/yyyy"); // Ngày hạch toán
                    worksheet.Cells[currentRow, 7].Value = invoice.PurchaseDate.ToString("dd/MM/yyyy"); // Ngày chứng từ
                    worksheet.Cells[currentRow, 8].Value = invoice.Code; // Số chứng từ
                    worksheet.Cells[currentRow, 9].Value = $"{invoice.Code}.1"; // Số phiếu xuất
                    worksheet.Cells[currentRow, 10].Value = ""; // Mẫu số HĐ
                    worksheet.Cells[currentRow, 11].Value = ""; // Ký hiệu HĐ
                    worksheet.Cells[currentRow, 12].Value = ""; // Số hoá đơn
                    worksheet.Cells[currentRow, 13].Value = ""; // Ngày hoá đơn
                    worksheet.Cells[currentRow, 14].Value = customerCode; // Mã khách hàng
                    worksheet.Cells[currentRow, 15].Value = customerName; // Tên khách hàng
                    worksheet.Cells[currentRow, 16].Value = invoice.CustomerName; // Địa chỉ
                    worksheet.Cells[currentRow, 17].Value = ""; // Mã số thuế
                    worksheet.Cells[currentRow, 18].Value = ""; // Đơn vị giao đại lý
                    worksheet.Cells[currentRow, 19].Value = ""; // Người nộp
                    worksheet.Cells[currentRow, 20].Value = ""; // Nạp vào TK
                    worksheet.Cells[currentRow, 21].Value = ""; // Tên ngân hàng
                    worksheet.Cells[currentRow, 22].Value = "Bán hàng"; // Diễn giải/Lý do nộp
                    worksheet.Cells[currentRow, 23].Value = "Xuất kho bán hàng"; // Lý do xuất
                    worksheet.Cells[currentRow, 24].Value = "VNĐ"; // Loại tiền
                    worksheet.Cells[currentRow, 25].Value = ""; // Tỉ giá
                    worksheet.Cells[currentRow, 26].Value = item.ProductCode; // Mã hàng
                    worksheet.Cells[currentRow, 27].Value = item.ProductName; // Tên hàng
                    worksheet.Cells[currentRow, 28].Value = "không"; // Là dòng ghi chú
                    worksheet.Cells[currentRow, 29].Value = "không"; // Hàng khuyến mại
                    worksheet.Cells[currentRow, 30].Value = $"131{branch.BranchCode.ToUpper()}"; // TK Tiền/Chi phí/Nợ
                    worksheet.Cells[currentRow, 31].Value = $"5111{branch.BranchCode.ToUpper()}"; // TK Doanh thu/Có
                    worksheet.Cells[currentRow, 32].Value = productDicKeyUnit.TryGetValue(item.ProductId, out var unit) ? unit : ""; // ĐVT
                    worksheet.Cells[currentRow, 33].Value = item.Quantity; // Số lượng
                    worksheet.Cells[currentRow, 34].Value = item.Price; // Đơn giá
                    worksheet.Cells[currentRow, 35].Value = Convert.ToDecimal(item.Quantity * Convert.ToDouble(item.Price)); // Thành tiền
                    worksheet.Cells[currentRow, 36].Value = ""; // Thành tiền quy đổi
                    worksheet.Cells[currentRow, 37].Value = item.DiscountRatio; // Tỷ lệ CK (%)
                    worksheet.Cells[currentRow, 38].Value = (item.DiscountRatio * Convert.ToDouble(item.Price)) / 100; // Tiền chiết khấu
                    worksheet.Cells[currentRow, 39].Value = ""; // Tiền chiết khấu quy đổi
                    worksheet.Cells[currentRow, 40].Value = $"5211{branch.BranchCode.ToUpper()}"; // TK chiết khấu
                    worksheet.Cells[currentRow, 41].Value = ""; // Giá tính thuế XK
                    worksheet.Cells[currentRow, 42].Value = ""; // % thuế xuất khẩu
                    worksheet.Cells[currentRow, 43].Value = ""; // Tiền thuế xuất khẩu
                    worksheet.Cells[currentRow, 44].Value = ""; // TK thuế xuất khẩu
                    worksheet.Cells[currentRow, 45].Value = ""; // % thuế GTGT  
                    worksheet.Cells[currentRow, 46].Value = ""; // % thuế suất KHAC
                    worksheet.Cells[currentRow, 47].Value = ""; // Tiền thuế GTGT
                    worksheet.Cells[currentRow, 48].Value = ""; // Tiền thuế GTGT quy đổi
                    worksheet.Cells[currentRow, 49].Value = ""; // TK thuế GTGT 
                    worksheet.Cells[currentRow, 50].Value = ""; // HH không TH trên tờ khai thuế GTGT
                    worksheet.Cells[currentRow, 51].Value = $"K{branch.BranchCode.ToUpper()}"; // Mã kho
                    worksheet.Cells[currentRow, 52].Value = $"632{branch.BranchCode.ToUpper()}"; // TK giá vốn
                    worksheet.Cells[currentRow, 53].Value = $"1561{branch.BranchCode.ToUpper()}"; // TK Kho
                    worksheet.Cells[currentRow, 54].Value = ""; // Đơn giá vốn
                    worksheet.Cells[currentRow, 55].Value = ""; // Tiền vốn
                    worksheet.Cells[currentRow, 56].Value = ""; // Hàng hóa giữ hộ/bán hộ
                }
            }
            return await package.GetAsByteArrayAsync();
        }
    }
}
