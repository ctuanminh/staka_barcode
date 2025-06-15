using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Be.Services.KiotViet
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
