using Be.Common.Branch.Response;
using Be.Common.customer;
using Be.Common.customer.Request;
using Be.Common.customer.Response;
using Be.Common.Dtos.Invoice;
using Be.Common.Responses;
using Be.Common.utils;
using Be.Core.Entities.Customer;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.Buffers.Text;
using System.Net.Http.Headers;
using System.Text.Json;
using Be.Data.Repository;
using Microsoft.EntityFrameworkCore;

namespace Be.Services.customer
{
    public class CustomerServiceImp : ServiceResponse, ICustomerService
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _config;
        private readonly IHttpClientFactory _httpClientFactory;
        private const string BaseUrl = "https://public.kiotapi.com/customers";
        private readonly IRepository _repository;
        public CustomerServiceImp(IConfiguration config, IHttpClientFactory httpClientFactory, IRepository repository)
        {
            _config = config;
            _httpClientFactory = httpClientFactory;
            _repository = repository;
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
            request.CurrentItem = request.CurrentItem <= 0 ? 0 : (request.CurrentItem - 1) * request.PageSize;

            var baseUrl = "https://public.kiotapi.com/customers";
            var url = QueryStringHelper.BuildQueryString(request, baseUrl);            
            var response = await _httpClient.GetAsync(url);
            if(!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                return BadRequest("Error", error);
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

        public Task<ApiResponse> ExportInvoiceMisa(SearchInvoiceRequest request, string templatePath)
        {
            throw new NotImplementedException();
        }
        public async Task<bool> SyncCustomer()
        {
            var customerList = new List<CustomerResponse>();
            var totalPages = 1;
            var currentPage = 1;
            const int pageSize = 200;

            var request = new SearchCustomerRequest
            {
                PageSize = pageSize,
                CurrentItem = 1,
            };

            do
            {
                request.CurrentItem = (currentPage - 1) * pageSize;
                var (success, content) = await KiotVietApiHelper.CallApiAsync(_httpClient, _config, BaseUrl, request);
                if (success && content != null)
                {
                    var customerData = JsonConvert.DeserializeObject<CustomerPagedResponse>(content);
                    customerList.AddRange(customerData.Data);
                    if (currentPage == 1 && customerData.Total > pageSize)
                        totalPages = (int)Math.Ceiling((double)customerData.Total / pageSize);
                }
                currentPage++;
            } while (currentPage <= totalPages);

            var customerExistList = await _repository.GetQueryable<Customer, long>()
                .Where(c => c.KiotId != 0)
                .ToDictionaryAsync(u => u.KiotId);

            if (customerList.Count == 0) return false;
            foreach (var customer in customerList)
            {
                if (customerExistList.TryGetValue(customer.Id, out var customerExist))
                {
                    customerExist.KiotId = customer.Id;
                    customerExist.Name = customer.Name;
                    customerExist.Code = customer.Code;
                    customerExist.Type = customerExist.Type;
                    customerExist.Address = customer.Address;
                    customerExist.Email = customer.Email;
                    customerExist.BirthDate = DateTime.SpecifyKind(customer.BirthDate, DateTimeKind.Utc);
                    customerExist.ContactNumber = customer.ContactNumber;
                    customerExist.Gender = customer.Gender;
                    customerExist.RetailerId = customer.RetailerId;
                    customerExist.CreatedAt = DateTime.SpecifyKind(customerExist.CreatedAt, DateTimeKind.Utc);
                    customerExist.CreatedBy = customerExist.CreatedBy;
                    customerExist.UpdatedAt = DateTime.SpecifyKind(customerExist.UpdatedAt, DateTimeKind.Utc);
                    customerExist.UpdatedBy = customerExist.UpdatedBy;
                    await _repository.UpdateAsync<Customer, long>(customerExist);
                }
                else
                {
                    var newCustomer = new Customer()
                    {
                        KiotId = customer.Id,
                        Name = customer.Name,
                        Code = customer.Code,
                        Type = customer.Type,
                        Address = customer.Address,
                        Email = customer.Email,
                        BirthDate = DateTime.SpecifyKind((DateTime)customer.BirthDate, DateTimeKind.Utc),
                        ContactNumber = customer.ContactNumber,
                        Gender = customer.Gender,
                        RetailerId = customer.RetailerId
                    };
                    await _repository.AddAsync<Customer, long>(newCustomer);
                }
            }
            await _repository.SaveChangeAsync();
            return true;
        }
    }
}
