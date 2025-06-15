using Microsoft.Extensions.Configuration;

namespace Be.Common.utils
{
    public static class KiotVietApiHelper
    {
        public static async Task<(bool Success, string Content)> CallApiAsync<TRequest>(
             HttpClient httpClient,
             IConfiguration config,
             string baseUrl,
             TRequest request,
             bool defaultToken = false)
        {
            try
            {
                var isHeaderReady = await KiotVietAuthHelper.AuthorizedHeadersAsync(httpClient, config, defaultToken);
                if (!isHeaderReady)
                {
                    return (false, "Token is not valid");
                }

                var url = QueryStringHelper.BuildQueryString(request, baseUrl);

                var response = await httpClient.GetAsync(url);
                if (!response.IsSuccessStatusCode)
                {
                    var error = await response.Content.ReadAsStringAsync();
                    return (false, error);
                }
                var dataResponse = await response.Content.ReadAsStringAsync();
                return (true, dataResponse);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            
        }
    }
}
