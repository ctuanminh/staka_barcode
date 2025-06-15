using System.Net.Http.Headers;
using System.Text.Json;
using Be.Common.Branch.Response;
using Be.Common.customer;
using Be.Common.customer.Request;
using Be.Common.customer.Response;
using Be.Common.Dtos.Invoice;
using Be.Common.Responses;
using Be.Common.utils;
using Microsoft.Extensions.Configuration;

namespace Be.Services.customer
{
    public class CustomerServiceImp : ServiceResponse, ICustomerService
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _config;
        private readonly IHttpClientFactory _httpClientFactory;

        public CustomerServiceImp(IConfiguration config, IHttpClientFactory httpClientFactory)
        {
            _config = config;
            _httpClientFactory = httpClientFactory;
            _httpClient = _httpClientFactory.CreateClient(); // Tạo HttpClient từ factory
        }

        public async Task<ApiResponse> GetAllCustomer(SearchCustomerRequest request)
        {
            var isHeaderReady = await PrepareAuthorizedHeadersAsync();
            if (!isHeaderReady)
            {
                return BadRequest("Token", "Token is not valid");
            }
            request.PageSize = request.PageSize <= 0 ? 10 : request.PageSize;
            request.currentItem = request.currentItem <= 0 ? 0 : (request.currentItem - 1) * request.PageSize;

            var baseUrl = "https://public.kiotapi.com/customers";
            var url = QueryStringHelper.BuildQueryString(request, baseUrl);            
            var response = await _httpClient.GetAsync(url);
            if(!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                return BadRequest("Error", error);
            }
            var responseData = await response.Content.ReadAsStringAsync();
            var customerApiResponse = JsonSerializer.Deserialize<CustomerPagedResponse>(responseData, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true 
            });
            
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
            var customerDetails = JsonSerializer.Deserialize<CustomerResponse>(responseData, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

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
            var branchDetails = JsonSerializer.Deserialize<BranchResponse>(responseData, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

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

        public async Task<Byte[]> GetAllInvoice(SearchInvoiceRequest request, string templatePath)
        {            
            string exportFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\Exports");
            string timestamp = DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss");
            
            string destFilename = Path.Combine(exportFolder, $"misa_export_{timestamp}.xlsx");

            File.Copy(templatePath, destFilename, true);

            var isHeaderReady = await PrepareAuthorizedHeadersAsync();
            if (!isHeaderReady)
            {
                return null;
            }
            request.PageSize = request.PageSize <= 0 ? 10 : request.PageSize;
            request.CurrentItem = request.CurrentItem <= 0 ? 0 : (request.CurrentItem - 1) * request.PageSize;

            var baseUrl = "https://public.kiotapi.com/invoices";
            var url = QueryStringHelper.BuildQueryString(request, baseUrl);
            var response = await _httpClient.GetAsync(url);
            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                return null;
            }
            var responseData = await response.Content.ReadAsStringAsync();
            var invoiceApiResponse = JsonSerializer.Deserialize<InvoiceApiResponse>(responseData, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            
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
                    var dataProduct = JsonSerializer.Deserialize<ProductInvoice>(responseDataProduct, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });
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

            var bytes = new byte[0];
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

                // With this corrected line:
                //workbook.SaveAs(destFilename);
                using var stream = new MemoryStream();
                workbook.SaveAs(stream);
                bytes = stream.ToArray();
            }
            return bytes;
        }

        public Task<ApiResponse> ExportInvoiceMisa(SearchInvoiceRequest request, string templatePath)
        {
            throw new NotImplementedException();
        }
    }
}
