using Be.Common.customer;
using Be.Common.customer.Dto;
using Be.Common.customer.Response;
using Be.Common.Dtos.Invoice;
using Be.Common.Request.KiotViet;
using Be.Common.Responses;
using Be.Common.Responses.KiotViet;
using Be.Common.utils;
using Be.Data.Repository;
using Be.Services.Order;
using Be.Services.Pos;
using DocumentFormat.OpenXml.Bibliography;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using PuppeteerSharp;

namespace Be.Services.Crm
{
    public class ReportServiceImp : ServiceResponse, IReportService
    {
        #region private
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _config;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IRepository _repository;
        private readonly IMemoryCache _memoryCache;
        private readonly IOrderService _orderService;
        private readonly IBranchService _branchService;
        #endregion

        #region ctor
        public ReportServiceImp(IConfiguration config,
            IHttpClientFactory httpClientFactory,
            IRepository repository,
            IMemoryCache memoryCache,
            IOrderService orderService,
            IBranchService branchService)
        {
            _config = config;
            _httpClientFactory = httpClientFactory;
            _httpClient = _httpClientFactory.CreateClient();
            _repository = repository;
            _memoryCache = memoryCache;
            _orderService = orderService;
            _branchService = branchService;
        }

        #endregion

        #region Methods

        
        public async Task<ApiResponse> GetSellReport(ReportSellRequest reportSellRequest)
        {
            var reports = await BuildSaleReportsAsync(reportSellRequest);
            return Ok(reports);
        }

        /// <summary>
        /// Export Sell Report to Excel file
        /// </summary>
        /// <param name="reportSellRequest"></param>
        /// <param name="templatePath"></param>
        /// <returns></returns>
        public async Task<Byte[]> ExportSellReport(ReportSellRequest reportSellRequest, string templatePath)
        {
            try
            {
                int currentYear = DateTime.Now.Year;
                int compareMonth = reportSellRequest.compareMonth;
                var currentMonth = DateTime.Now.Month;

                var compareMonthDate = (compareMonth == DateTime.Now.Month)
                ? DateTime.Now
                : new DateTime(currentYear, compareMonth, DateTime.DaysInMonth(currentYear, compareMonth));

                DateTime toCompareDate = new(currentYear, compareMonth, DateTime.DaysInMonth(currentYear, compareMonth));

                var file = await File.ReadAllBytesAsync(templatePath);
                using var stream = new MemoryStream(file);
                var response = await BuildSaleReportsAsync(reportSellRequest);
                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                using (var package = new ExcelPackage(stream))
                {
                    ExcelWorksheet worksheet = package.Workbook.Worksheets[0];
                    worksheet.Name = "BC bán hàng T" + reportSellRequest.compareMonth;
                    int startRow = 4;
                    worksheet.Cells[1, 1].Value = $"BẢNG TỔNG HỢP KẾT QUẢ BÁN HÀNG NHÂN VIÊN KINH DOANH\r\nNgày {compareMonthDate.Day} THÁNG {compareMonth:D2} NĂM {DateTime.Now:yyyy}";
                    worksheet.Cells[3, 5].Value = $"Average of Doanh thu thuần tháng {reportSellRequest.referenceMonth} (theo người bán)";
                    worksheet.Cells[3, 6].Value = $"Average of Doanh thu thuần tháng {reportSellRequest.compareMonth} (theo người bán)";
                    worksheet.Cells[3, 11].Value = $"Tỷ lệ trả hàng tháng {reportSellRequest.compareMonth}";
                    foreach (var item in response)
                    {
                        int currentRow = startRow;
                        worksheet.InsertRow(startRow, 1, 4); // Chèn một hàng mới
                        worksheet.Cells[currentRow, 1].Value = item.SaleName; // Tên nhân viên bán hàng
                        worksheet.Cells[currentRow, 2].Value = item.SellCount; // Số lượng đơn hàng bán ra
                        worksheet.Cells[currentRow, 3].Value = item.ReturnCount; // Số lượng đơn hàng hoàn trả
                        worksheet.Cells[currentRow, 4].Value = item.ReturnTotalAmount; // Tổng số tiền hoàn trả
                        worksheet.Cells[currentRow, 4].Style.Numberformat.Format = "#,##0";

                        worksheet.Cells[currentRow, 5].Value = item.RevenueMonth3; // Doanh thu tháng 3
                        worksheet.Cells[currentRow, 5].Style.Numberformat.Format = "#,##0";

                        worksheet.Cells[currentRow, 6].Value = item.RevenueMonth4; // Doanh thu tháng 4
                        worksheet.Cells[currentRow, 6].Style.Numberformat.Format = "#,##0";

                        worksheet.Cells[currentRow, 7].Value = item.TotalCustomers; // Tổng số khách hàng
                        worksheet.Cells[currentRow, 8].Value = item.OrderSuccessRate; // Tỷ lệ đơn hàng thành công

                        worksheet.Cells[currentRow, 9].Value = item.AvgTransactionValue; // Giá trị giao dịch trung bình
                        worksheet.Cells[currentRow, 9].Style.Numberformat.Format = "#,##0";

                        worksheet.Cells[currentRow, 10].Value = item.RevenueGrowthPercent; // Tỷ lệ tăng trưởng doanh thu
                        worksheet.Cells[currentRow, 11].Value = item.ReturnRate; // Tỷ lệ hoàn trả
                        worksheet.Cells[currentRow, 12].Value = item.BranchName; // Tên chi nhánh
                        currentRow++;
                    }
                    int sumRow = startRow + response.Count; // Tính toán vị trí hàng tổng
                    worksheet.Cells[sumRow, 1].Value = "Tổng hợp";
                    worksheet.Cells[sumRow, 1, sumRow, 11].Style.Font.Bold = true;

                    worksheet.Cells[sumRow, 2].Formula = $"SUM(B{startRow}:B{sumRow - 1})";
                    worksheet.Cells[sumRow, 3].Formula = $"SUM(C{startRow}:C{sumRow - 1})";
                    worksheet.Cells[sumRow, 4].Formula = $"SUM(D{startRow}:D{sumRow - 1})";
                    worksheet.Cells[sumRow, 5].Formula = $"SUM(E{startRow}:E{sumRow - 1})";
                    worksheet.Cells[sumRow, 6].Formula = $"SUM(F{startRow}:F{sumRow - 1})";
                    worksheet.Cells[sumRow, 7].Formula = $"SUM(G{startRow}:G{sumRow - 1})";
                    worksheet.Cells[sumRow, 8].Value = "-";
                    worksheet.Cells[sumRow, 9].Value = "-";
                    worksheet.Cells[sumRow, 10].Value = "-";
                    worksheet.Cells[sumRow, 11].Value = "-";
                    worksheet.Cells[sumRow, 12].Value = "-";

                    worksheet.Cells[sumRow, 2].Style.Numberformat.Format = "#,##0";
                    worksheet.Cells[sumRow, 3].Style.Numberformat.Format = "#,##0";
                    worksheet.Cells[sumRow, 4].Style.Numberformat.Format = "#,##0";
                    worksheet.Cells[sumRow, 5].Style.Numberformat.Format = "#,##0";
                    worksheet.Cells[sumRow, 6].Style.Numberformat.Format = "#,##0";
                    worksheet.Cells[sumRow, 7].Style.Numberformat.Format = "#,##0";

                    int endRow = startRow + response.Count;
                    int endCol = 12;

                    var dataRange = worksheet.Cells[startRow, 1, endRow, endCol];
                    dataRange.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                    dataRange.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                    dataRange.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                    dataRange.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                    return await package.GetAsByteArrayAsync();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Build sale report
        /// </summary>
        /// <param name="reportSellRequest"></param>
        /// <returns></returns>
        private async Task<List<SaleReport>> BuildSaleReportsAsync(ReportSellRequest reportSellRequest)
        {
            int currentYear = DateTime.Now.Year;
            int compareMonth = reportSellRequest.compareMonth;
            int referenceMonth = reportSellRequest.referenceMonth;
            var baseUrl = "https://public.kiotapi.com/invoices";
            var returnUrl = "https://public.kiotapi.com/returns";

            DateTime fromCompareDate = new(currentYear, compareMonth, 1);
            DateTime toCompareDate = new(currentYear, compareMonth, DateTime.DaysInMonth(currentYear, compareMonth));
            DateTime fromReferenceDate = new(currentYear, referenceMonth, 1);
            DateTime toReferenceDate = new(currentYear, referenceMonth, DateTime.DaysInMonth(currentYear, referenceMonth));

            // Prepare requests
            var invoiceCompareRequest = new SearchInvoiceRequest
            {
                FromPurchaseDate = fromCompareDate,
                ToPurchaseDate = toCompareDate,
                PageSize = 100,
                CurrentItem = 1,
                OrderBy = "createdDate",
                OrderDirection = "Desc",
                Status = [1],
                BranchIds = reportSellRequest.BranchIds
            };
            var invoiceReferenceRequest = new SearchInvoiceRequest
            {
                FromPurchaseDate = fromReferenceDate,
                ToPurchaseDate = toReferenceDate,
                PageSize = 100,
                CurrentItem = 1,
                Status = [1],
                OrderBy = "createdDate",
                OrderDirection = "Desc",
                BranchIds = reportSellRequest.BranchIds
            };

            var invoiceReturnRequest = new SearchInvoiceReturnRequest
            {
                FromReturnDate = fromReferenceDate,
                ToReturnDate = toReferenceDate,
                PageSize = 100,
                CurrentItem = 1,
                OrderBy = "createdDate",
                OrderDirection = "Desc",
            };
            var invoiceCompareTask = GetInvoicesAsync(invoiceCompareRequest, baseUrl);
            var invoiceReferenceTask = GetInvoicesAsync(invoiceReferenceRequest, baseUrl);
            var invoiceReturnTask = GetReturnAsync(invoiceReturnRequest, returnUrl);
            await Task.WhenAll(invoiceCompareTask, invoiceReferenceTask, invoiceReturnTask);
            var listInvoiceCompareRequest = invoiceCompareTask.Result;
            var listInvoiceReferenceRequest = invoiceReferenceTask.Result;
            var listInvoiceReturnRequest = invoiceReturnTask.Result;

            // Gộp hóa đơn bán hàng theo người bán 
            var groupedSells = listInvoiceCompareRequest
                .GroupBy(i => (i.SoldByName ?? "-").Trim().ToUpper())
                .ToDictionary(
                    g => g.Key,
                    g => new
                    {
                        SellName = g.FirstOrDefault()?.SoldByName ?? "-",
                        SellCount = g.Count(),
                        RevenueMonth = g.Sum(x => x.Total), // doanh thu tháng 4                        
                        AvgTransactionValue = Math.Round(g.Sum(x => x.Total) / g.Count()), // giá trị trung bình giao dịch
                        TotalCustomers = g.Select(i => i.CustomerId).Distinct().Count(), // số lượng khách hàng
                        BranchName = g.FirstOrDefault()?.BranchName ?? "-"
                    },
                    StringComparer.OrdinalIgnoreCase
                );

            // Gộp hóa đơn bán hàng theo người bán 
            var groupedReference = listInvoiceReferenceRequest
                .GroupBy(i => (i.SoldByName ?? "-").Trim().ToUpper())
                .ToDictionary(
                    g => g.Key,
                    g => new
                    {
                        SellName = g.FirstOrDefault()?.SoldByName ?? "-",
                        SellCount = g.Count(),
                        RevenueMonthReference = g.Sum(x => x.Total), // doanh thu tháng 4                        
                        AvgTransactionValue = g.Average(x => x.Total) // giá trị trung bình giao dịch
                    },
                    StringComparer.OrdinalIgnoreCase
                );
            // gộp hàng trả lại theo người bán
            var groupedReturns = listInvoiceReturnRequest
                .Where(
                    r => reportSellRequest.BranchIds == null || reportSellRequest.BranchIds.Length == 0 || reportSellRequest.BranchIds.Contains(r.BranchId)
                    && r.Status == 1)
                .GroupBy(r => r.SoldByName.ToUpper())
                .ToDictionary(
                    g => g.Key,
                    g => new
                    {
                        ReturnTotalQuantity = g.Count(),
                        ReturnTotalAmount = g.SelectMany(x => x.ReturnDetails ?? Enumerable.Empty<ReturnDetail>())
                            .Sum(d => d.SubTotal)
                    });

            var reports = groupedSells.Select(kvp =>
            {
                var sale = kvp.Key;
                var sell = kvp.Value;

                groupedReference.TryGetValue(sale.ToUpper(), out var referent);
                groupedReturns.TryGetValue(sale.ToUpper(), out var ret);

                int sellCount = sell?.SellCount ?? 0;
                decimal revenueMonthReference = referent?.RevenueMonthReference ?? 0;
                double returnCount = ret?.ReturnTotalQuantity ?? 0;
                decimal returnAmount = ret?.ReturnTotalAmount ?? 0;

                return new SaleReport
                {
                    SaleName = sell?.SellName ?? "-",
                    SellCount = sellCount, // số lượng bán
                    ReturnCount = returnCount, // số lượng trả lại
                    ReturnTotalAmount = returnAmount, // tổng tiền trả lại
                    RevenueMonth4 = sell.RevenueMonth, // doanh thu tháng 4
                    RevenueMonth3 = revenueMonthReference, // doanh thu tháng 4
                    TotalCustomers = sell?.TotalCustomers ?? 0, // tổng số khách hàng
                    AvgTransactionValue = sell.AvgTransactionValue, // giá trị giao dịch trung bình
                    ReturnRate = sellCount > 0 ? Math.Round((decimal)returnCount / sellCount * 100, 1) : 0, // tỷ lệ trả hàng
                    // tỷ lệ tăng trưởng doanh thu
                    RevenueGrowthPercent = revenueMonthReference > 0 ? Math.Round((sell.RevenueMonth - revenueMonthReference) / revenueMonthReference * 100, 1) : 0,
                    BranchName = sell.BranchName // Tên chi nhánh
                };
            }).ToList();
            return reports;
        }

        /// <summary>
        /// Get Invoices from KiotViet API with caching
        /// </summary>
        /// <param name="request"></param>
        /// <param name="baseUrl"></param>
        /// <returns></returns>
        private async Task<List<InvoiceResponse>> GetInvoicesAsync(SearchInvoiceRequest request, string baseUrl)
        {
            // Tạo cache key chi tiết theo branch, ngày, pagesize, order
            string branchKey = request.BranchIds != null && request.BranchIds.Any()
                ? string.Join("_", request.BranchIds)
                : "all";
            string cacheKey = $"invoices_{branchKey}_{request.FromPurchaseDate:yyyyMMdd}_{request.ToPurchaseDate:yyyyMMdd}_{request.PageSize}_{request.OrderBy}_{request.OrderDirection}";

            // Kiểm tra cache trước
            if (_memoryCache.TryGetValue(cacheKey, out List<InvoiceResponse> cachedInvoices))
            {
                return cachedInvoices;
            }

            var result = new List<InvoiceResponse>();
            int totalPages = 1;
            int currentPage = 1;
            int pageSize = request.PageSize;

            do
            {
                request.CurrentItem = (currentPage - 1) * pageSize;
                var (Success, Content) = await KiotVietApiHelper.CallApiAsync(_httpClient, _config, baseUrl, request);
                if (Success && Content != null)
                {
                    var invoiceData = JsonConvert.DeserializeObject<InvoiceApiResponse>(Content);
                    result.AddRange(invoiceData.Data);
                    if (currentPage == 1 && invoiceData.Total > pageSize)
                        totalPages = (int)Math.Ceiling((double)invoiceData.Total / pageSize);
                }
                currentPage++;
            } while (currentPage <= totalPages);

            // Lưu vào cache với thời gian sống ngắn (ví dụ: 5 phút)
            _memoryCache.Set(cacheKey, result, TimeSpan.FromMinutes(15));

            return result;
        }

        /// <summary>
        /// Get Return Invoices from KiotViet API with caching
        /// </summary>
        /// <param name="request"></param>
        /// <param name="baseUrl"></param>
        /// <returns></returns>
        private async Task<List<ReturnInvoiceResponse>> GetReturnAsync(SearchInvoiceReturnRequest request, string baseUrl)
        {
            // Tạo cache key chi tiết theo branch, ngày, pagesize, order
            string branchKey = request.BranchIds != null && request.BranchIds.Any()
                ? string.Join("_", request.BranchIds)
                : "all";
            string cacheKey = $"returnInvoices_{branchKey}_{request.FromReturnDate:yyyyMMdd}_{request.ToReturnDate:yyyyMMdd}_{request.PageSize}_{request.OrderBy}_{request.OrderDirection}";

            // Kiểm tra cache trước
            if (_memoryCache.TryGetValue(cacheKey, out List<ReturnInvoiceResponse> cachedInvoices))
            {
                return cachedInvoices;
            }

            var result = new List<ReturnInvoiceResponse>();
            int totalPages = 1;
            int currentPage = 1;
            int pageSize = request.PageSize;

            do
            {
                request.CurrentItem = (currentPage - 1) * pageSize;
                var (Success, Content) = await KiotVietApiHelper.CallApiAsync(_httpClient, _config, baseUrl, request);
                if (Success && Content != null)
                {
                    var invoiceData = JsonConvert.DeserializeObject<ReturnInvoiceApiResponse>(Content);
                    result.AddRange(invoiceData.Data);
                    if (currentPage == 1 && invoiceData.Total > pageSize)
                        totalPages = (int)Math.Ceiling((double)invoiceData.Total / pageSize);
                }
                currentPage++;
            } while (currentPage <= totalPages);

            // Lưu vào cache với thời gian sống ngắn (ví dụ: 5 phút)
            _memoryCache.Set(cacheKey, result, TimeSpan.FromMinutes(15));

            return result;
        }

        public async Task<string> LoginAndGetTokenAsync(string username, string password)
        {
            await new BrowserFetcher().DownloadAsync();

            var browser = await Puppeteer.LaunchAsync(new LaunchOptions
            {
                Headless = true
            });

            var page = await browser.NewPageAsync();
            await page.GoToAsync("https://stakapro.kiotviet.vn/man/#/login");

            await page.TypeAsync("input[name='UserName']", username);
            await page.TypeAsync("input[name='Password']", password);
            await page.ClickAsync("input[type='submit']");

            // Đợi token xuất hiện trong localStorage (tối đa 30s)
            await page.WaitForFunctionAsync(
                @"() => window.localStorage.getItem('f-accessToken') !== null",
                new WaitForFunctionOptions { Timeout = 30000 }
            );

            var token = await page.EvaluateExpressionAsync<string>(
                "window.localStorage.getItem('f-accessToken')"
            );

            await browser.CloseAsync();

            // Xử lý trường hợp đăng nhập thất bại
            if (string.IsNullOrEmpty(token))
                throw new Exception("Đăng nhập thất bại hoặc không lấy được token.");

            return token;
        }

        public async Task<ApiResponse> GetCustomerReport(ReportCustomerRequest reportCustomerRequest)
        {            
            var reports = await BuildCustomerReportsAsync(reportCustomerRequest);
            return Ok(reports);
        }

        public async Task<Byte[]> ExportCustomerReport(ReportCustomerRequest reportCustomerRequest, string templatePath)
        {
            try
            {
                int currentYear = DateTime.Now.Year;
                int compareMonth = reportCustomerRequest.compareMonth;
                DateTime toCompareDate = new(currentYear, compareMonth, DateTime.DaysInMonth(currentYear, compareMonth));
                var file = await File.ReadAllBytesAsync(templatePath);
                using var stream = new MemoryStream(file);
                var response = await BuildCustomerReportsAsync(reportCustomerRequest);
                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                using (var package = new ExcelPackage(stream))
                {
                    ExcelWorksheet worksheet = package.Workbook.Worksheets[0];
                    worksheet.Name = "BC khách hàng T" + reportCustomerRequest.compareMonth;
                    int startRow = 10;                    
                    foreach (var item in response)
                    {
                        int currentRow = startRow;
                        worksheet.InsertRow(startRow, 1, 4); // Chèn một hàng mới
                        worksheet.Cells[currentRow, 2].Value = item.GroupName; // Tên nhóm khách hàng
                        worksheet.Cells[currentRow, 3].Value = item.TotalCustomers; // Số lượng khách hàng trong tháng so sánh
                        worksheet.Cells[currentRow, 4].Value = item.TotalCustomersReference; // Số lượng khách hàng trong tháng tham chiếu
                        worksheet.Cells[currentRow, 5].Value = item.CustomerGrowthPercent; // Tỷ lệ tăng trưởng khách hàng
                        worksheet.Cells[currentRow, 6].Value = item.CustomerNoBuybackPercent; // Doanh thu tháng tham chiếu
                        worksheet.Cells[currentRow, 7].Value = item.CustomerBuybackPercent;
                        worksheet.Cells[currentRow, 8].Value = item.RevenueMonth; // Doanh thu tháng so sánh
                        worksheet.Cells[currentRow, 9].Value = item.RevenueMonthReference; // Doanh thu tháng so sánh
                        worksheet.Cells[currentRow, 10].Value = item.CustomerGroupRevenueRateMonth; // Tỉ lệ doanh thu nhóm khách hàng/ tổng doanh thu chi nhánh của tháng hiện tại
                        worksheet.Cells[currentRow, 11].Value = item.RevenueGrowthRate; // Tỷ lệ tăng trưởng doanh thu
                    }
                    return await package.GetAsByteArrayAsync();
                };                              
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private async Task<List<CustomerReport>> BuildCustomerReportsAsync(ReportCustomerRequest reportCustomerRequest)
        {
            int currentYear = DateTime.Now.Year;
            int compareMonth = reportCustomerRequest.compareMonth;
            int referenceMonth = reportCustomerRequest.referenceMonth;
            var baseUrl = "https://public.kiotapi.com/customers";            
            var invoiceUrl = "https://public.kiotapi.com/invoices";

            var branchIds = reportCustomerRequest.BranchIds;
            reportCustomerRequest.BranchIds = null; // Tắt lọc theo chi nhánh để lấy tất cả khách hàng

            DateTime fromCompareDate = new(currentYear, compareMonth, 1, 0, 0, 0);            
            DateTime toCompareDate = new(currentYear, compareMonth, DateTime.DaysInMonth(currentYear, compareMonth), 23, 59, 59);
            DateTime fromReferenceDate = new(currentYear, referenceMonth, 1, 0, 0, 0);
            DateTime toReferenceDate = new(currentYear, referenceMonth, DateTime.DaysInMonth(currentYear, referenceMonth), 23, 59, 59);

            // Prepare requests
            reportCustomerRequest.PageSize = 100; // Set default page size
            reportCustomerRequest.CurrentItem = 1; // Set default current item
            reportCustomerRequest.Status = [1]; // Chỉ lấy khách hàng đang hoạt động

            // Prepare requests
            var invoiceCompareRequest = new SearchInvoiceRequest
            {
                FromPurchaseDate = fromCompareDate,
                ToPurchaseDate = toCompareDate,
                PageSize = 100,
                CurrentItem = 1,
                OrderBy = "createdDate",
                OrderDirection = "Desc",
                Status = [1],
                BranchIds = branchIds
            };
            var invoiceReferenceRequest = new SearchInvoiceRequest
            {
                FromPurchaseDate = fromReferenceDate,
                ToPurchaseDate = toReferenceDate,
                PageSize = 100,
                CurrentItem = 1,
                Status = [1],
                OrderBy = "createdDate",
                OrderDirection = "Desc",
                BranchIds = branchIds
            };


            var customerCompareTask = GetCustomerAsync(reportCustomerRequest, baseUrl);
            var invoiceCompareTask = GetInvoicesAsync(invoiceCompareRequest, invoiceUrl);
            var invoiceReferenceTask = GetInvoicesAsync(invoiceReferenceRequest, invoiceUrl);

            await Task.WhenAll(customerCompareTask, invoiceCompareTask, invoiceReferenceTask);
            var listCustomerCompare = customerCompareTask.Result;
            var listInvoiceCompareRequest = invoiceCompareTask.Result; // Danh sách hóa đơn bán hàng trong tháng so sánh
            var listInvoiceReferenceRequest = invoiceReferenceTask.Result; // Danh sách hóa đơn bán hàng trong tháng tham chiếu

            // Lấy danh sách chi nhánh
            var branches = await _branchService.GetAllBranches();

            // Lọc ra customer của chi nhánh trong danh sách khách hàng
            var listCustomerMonthOfBanch = listCustomerCompare
                .Where(c => reportCustomerRequest.BranchIds == null 
                    || reportCustomerRequest.BranchIds.Length == 0 
                    || reportCustomerRequest.BranchIds.Contains(c.BranchId))
                .ToList();

            //Danh sách nhóm khách hàng : Cột 1(Nhóm kháchh hàng)
            var allGrounp = listCustomerCompare
                    .Where(i => !string.IsNullOrEmpty(i.Groups))
                    .Select(i => i.Groups.Trim().ToUpper())
                    .Distinct()
                    .ToList();

            // Đếm số khách hàng theo nhóm trong tháng cần báo cáo : Cột 2(Khách hàng tháng xem)
            var customerOfMonth = allGrounp.ToDictionary(
                g => g,
                g => listCustomerCompare
                .Where(i => !string.IsNullOrEmpty(i.Groups)
                    && i.Groups.Trim().Equals(g, StringComparison.CurrentCultureIgnoreCase)
                    && i.CreatedDate >= fromCompareDate
                    && i.CreatedDate <= toCompareDate).Count(),
                StringComparer.OrdinalIgnoreCase
            );

            // Đếm số khách hàng theo nhóm trong tháng cần đối chiếu. Cột 3(Khách hàng của tháng so đối chiếu)
            var customerMonthReference = allGrounp.ToDictionary(
                g => g,
                g => listCustomerCompare
                .Where(i => !string.IsNullOrEmpty(i.Groups)
                    && i.Groups.Trim().Equals(g, StringComparison.CurrentCultureIgnoreCase)
                    && i.CreatedDate >= fromReferenceDate
                    && i.CreatedDate <= toReferenceDate).Count(),
                StringComparer.OrdinalIgnoreCase
            );
            
            // cột 7: Đếm được bao nhiêu khách hàng thuộc nhóm kh có phát sinh trong tháng.
            //B1. join danh listInvoiceCompareRequest và listCustomerCompare để đếm số lượt mua hàng của khách hàng.          
            var numCustomerByGroupHasInvoiceOfMonth = listInvoiceCompareRequest
                .GroupBy(i => i.CustomerId)
                .Select(g =>
                {
                    var customer = listCustomerCompare.FirstOrDefault(c => c.Id == g.Key);
                    return new
                    {
                        CustomerId = g.Key,
                        Groups = customer?.Groups?.Trim().ToUpper() ?? "-",
                    };
                })
                .GroupBy(i => i.Groups)
                .Select(g => new
                {
                    Groups = g.Key.Trim().ToUpper(),
                    CustomerCount = g.Count()
                }).ToList();

            
            // Tính tổng doanh thu khách hàng theo nhóm trong tháng cần đối chiếu.            
            var revenueMonthByGroup = listInvoiceCompareRequest
                .GroupBy(i => i.CustomerId)
                .Select(g =>
                {
                    var customer = listCustomerCompare.FirstOrDefault(c => c.Id == g.Key);
                    return new
                    {
                        CustomerId = g.Key,
                        Groups = customer?.Groups?.Trim().ToUpper() ?? "-",
                        Total = g.Sum(x => x.Total) // Tổng doanh thu của khách hàng trong tháng
                    };
                })
                .GroupBy(i => i.Groups)
                .Select(g => new 
                {
                    Groups = g.Key.Trim().ToUpper(),
                    RevenueMonth = g.Sum(x => x.Total) // Tổng doanh thu theo nhóm khách hàng
                }).ToList();

            var revenueMonthByGroupReference = listInvoiceReferenceRequest
                .GroupBy(i => i.CustomerId)
                .Select(g =>
                {
                    var customer = listCustomerCompare.FirstOrDefault(c => c.Id == g.Key && c.Groups != null);
                    return new
                    {
                        CustomerId = g.Key,
                        Groups = customer?.Groups?.Trim().ToUpper() ?? "-",
                        Total = g.Sum(x => x.Total) // Tổng doanh thu của khách hàng trong tháng tham chiếu
                    };
                })
                .GroupBy(i => i.Groups)
                .Select(g => new
                {
                    Groups = g.Key.Trim().ToUpper(),
                    RevenueMonthReference = g.Sum(x => x.Total) // Tổng doanh thu theo nhóm khách hàng trong tháng tham chiếu
                }).ToList();

            //Lọc khách hàng chưa set nhóm KH
            var customerUnGroup = listCustomerCompare.Where(x => x.Groups == "" || x.Groups == null).ToList();
          
            var result = allGrounp.Select(g =>
            {
                var groupName = g.Trim().ToUpper();
                var customerCount = customerOfMonth.TryGetValue(groupName, out int count) ? count : 0; // Số lượng khách hàng trong tháng
                var customerCountReference = customerMonthReference.TryGetValue(groupName, out int countRef) ? countRef : 0; // Số lượng khách hàng trong tháng tham chiếu
                var revenueMonth = revenueMonthByGroup.FirstOrDefault(x => x.Groups.Equals(groupName, StringComparison.OrdinalIgnoreCase))?.RevenueMonth ?? 0; // Doanh thu thán theo nhóm khách hàng
                var revenueMonthReference = revenueMonthByGroupReference.FirstOrDefault(x => x.Groups.Equals(groupName, StringComparison.OrdinalIgnoreCase))?.RevenueMonthReference ?? 0; // Doanh thu tháng tham chiếu theo nhóm khách hàng
                var totalCustomersBuyBackOfMonth = numCustomerByGroupHasInvoiceOfMonth.FirstOrDefault(x => x.Groups.Equals(groupName, StringComparison.OrdinalIgnoreCase))?.CustomerCount ?? 0; // Tổng số khách hàng có phát sinh trong tháng theo nhóm
                return new CustomerReport
                {
                    GroupName = groupName, // Tên nhóm khách hàng
                    TotalCustomersReference = customerCountReference, // Tổng số khách hàng trong tháng tham chiếu
                    TotalCustomers = customerCount, // Tổng số khách hàng trong tháng
                    CustomerGrowthPercent = customerCountReference != 0
                        ? (customerCount / customerCountReference) * 100 : 0, // Tỷ lệ tăng trưởng khách hàng
                    CustomerBuybackPercent = customerCount != 0 ? (totalCustomersBuyBackOfMonth / customerCount) * 100 : 0, // Tỷ lệ khách hàng mua lại trong tháng
                    CustomerNoBuybackPercent = customerCount != 0? ((customerCount - totalCustomersBuyBackOfMonth) / customerCount) * 100 : 0, // Tỷ lệ khách hàng không mua lại trong tháng
                    RevenueMonth = revenueMonth, // Doanh thu tháng
                    RevenueMonthReference = revenueMonthReference, // Doanh thu tháng tham chiếu
                    CustomerGroupRevenueRateMonth = revenueMonthReference != 0? revenueMonth / revenueMonthReference : 0, //Tỉ lệ doanh thu nhóm khách hàng/ tổng doanh thu chi nhánh của tháng hiện tại
                    RevenueGrowthRate = revenueMonthReference != 0? (revenueMonth / revenueMonthReference) * 100 : 0, // Tỉ lệ tăng trưởng doanh thu
                };
            }).ToList();        
            return result;
        }

        private async Task<List<CustomerResponse>> GetCustomerAsync(ReportCustomerRequest request, string baseUrl)
        {
            // Tạo cache key chi tiết theo branch, ngày, pagesize, order
            string branchKey = request.BranchIds != null && request.BranchIds.Any()
                ? string.Join("_", request.BranchIds)
                : "all";
            string cacheKey = $"customer_{branchKey}_{request.CreatedDate:yyyyMMdd}_{request.CreatedDate:yyyyMMdd}_{request.PageSize}_{request.OrderBy}_{request.OrderDirection}";

            // Kiểm tra cache trước
            if (_memoryCache.TryGetValue(cacheKey, out List<CustomerResponse> cachedCustomer))
            {
                return cachedCustomer;
            }

            var result = new List<CustomerResponse>();
            int totalPages = 1;
            int currentPage = 1;
            int pageSize = request.PageSize;

            do
            {
                request.CurrentItem = (currentPage - 1) * pageSize;
                var (Success, Content) = await KiotVietApiHelper.CallApiAsync(_httpClient, _config, baseUrl, request);
                if (Success && Content != null)
                {
                    var customerData = JsonConvert.DeserializeObject<CustomerPagedResponse>(Content);
                    result.AddRange(customerData.Data);
                    if (currentPage == 1 && customerData.Total > pageSize)
                        totalPages = (int)Math.Ceiling((double)customerData.Total / pageSize);
                }
                currentPage++;
            } while (currentPage <= totalPages);
            
            _memoryCache.Set(cacheKey, result, TimeSpan.FromMinutes(15));

            return result;
        }
        #endregion

    }
}
