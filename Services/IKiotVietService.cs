using Microsoft.Extensions.Configuration;

namespace Services
{
    public interface IKiotVietService
    {
        Task<string> GetOrderByCodeAsync(string code);

        Task<(bool Success, string Content)> CallApiAsync<TRequest>(
            string baseUrl,
            TRequest? request,
            string method = "GET",
            bool defaultToken = false);
    }
}
