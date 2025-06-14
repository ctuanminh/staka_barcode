using Microsoft.Extensions.Configuration;
using System.Net.Http.Headers;
using System.Text.Json;

namespace Services
{
    public class KiotVietServiceImp : IKiotVietService
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _config;

        public KiotVietServiceImp(HttpClient httpClient, IConfiguration config)
        {
            _httpClient = httpClient;
            _config = config;
        }

        private async Task<string?> GetAccessTokenAsync()
        {
            var tokenUrl = _config["KiotViet:TokenUrl"];
            var tokenRequest = new FormUrlEncodedContent([
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

            var tokenData = await response.Content.ReadAsStringAsync();
            var tokenJson = JsonDocument.Parse(tokenData);
            return tokenJson.RootElement.GetProperty("access_token").GetString();
        }

        public async Task<bool> AuthorizedHeadersAsync(bool defaultToken = false)
        {
            string? token;
            if (defaultToken)
            {
                token = _config["KiotViet:TokenDefault"];
            }
            else
            {
                token = await GetAccessTokenAsync();
            }

            if (string.IsNullOrEmpty(token))
            {
                return false;
            }

            _httpClient.DefaultRequestHeaders.Clear();
            _httpClient.DefaultRequestHeaders.Add("Retailer", _config["KiotViet:Retailer"]);
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            return true;
        }

        public async Task<string> GetOrderByCodeAsync(string code)
        {
            throw new NotImplementedException();
        }

        public async Task<(bool Success, string Content)> CallApiAsync<TRequest>(string baseUrl, TRequest? request,
            string method = "GET", bool defaultToken = false)
        {
            try
            {
                var isHeaderReady = await AuthorizedHeadersAsync(defaultToken);
                if (!isHeaderReady)
                {
                    return (false, "Token is not valid");
                }

                var url = request != null ? QueryStringHelper.BuildQueryString(request, baseUrl) : baseUrl;
                
                if (method != "GET")
                {
                    return (true, "");
                }
                else
                {
                    var response = await _httpClient.GetAsync(url);
                    if (!response.IsSuccessStatusCode)
                    {
                        var error = await response.Content.ReadAsStringAsync();
                        return (false, error);
                    }

                    var dataResponse = await response.Content.ReadAsStringAsync();
                    return (true, dataResponse);
                }

            }
            catch (Exception ex)
            {
                return (false, ex.Message);
            }
        }

    }
}
