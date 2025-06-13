using Microsoft.Extensions.Configuration;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;

namespace Services
{
    public class KiotVietAuthHelper
    {
        public static async Task<string> GetAccessTokenAsync(HttpClient httpClient, IConfiguration config)
        {
            var tokenUrl = config["KiotViet:TokenUrl"];
            var tokenRequest = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("client_id", config["KiotViet:ClientId"]),
                new KeyValuePair<string, string>("client_secret", config["KiotViet:ClientSecret"]),
                new KeyValuePair<string, string>("grant_type", "client_credentials"),
                new KeyValuePair<string, string>("scope", "PublicApi.Access")
            });

            var response = await httpClient.PostAsync(tokenUrl, tokenRequest);
            if (!response.IsSuccessStatusCode)
            {
                return null;
            }

            var tokenData = await response.Content.ReadAsStringAsync();
            var tokenJson = JsonDocument.Parse(tokenData);
            return tokenJson.RootElement.GetProperty("access_token").GetString();
        }

        public static async Task<bool> AuthorizedHeadersAsync(HttpClient httpClient, IConfiguration config, bool defaultToken = false)
        {
            var token = "";
            //Default dành cho các trường hợp muốn lấy dữ liệu từ kiot mà trong API public không có
            //Bắt buộc Đăng nhập kiotviet lấy token
            if (defaultToken)
            {
                token = config["KiotViet:TokenDefault"];
            }
            else
            {
                token = await GetAccessTokenAsync(httpClient, config);
            }
            if (string.IsNullOrEmpty(token))
            {
                return false;
            }

            httpClient.DefaultRequestHeaders.Clear();
            httpClient.DefaultRequestHeaders.Add("Retailer", config["KiotViet:Retailer"]);
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            return true;
        }

        /// <summary>
        /// Mã hóa chuỗi bằng AES với khoá và IV đã cho.
        /// </summary>
        /// <param name="plainText"></param>
        /// <param name="key"></param>
        /// <param name="iv"></param>
        /// <returns></returns>

        public static string Encrypt(string plainText, string key, string iv)
        {
            if (string.IsNullOrEmpty(plainText) || string.IsNullOrEmpty(key) || string.IsNullOrEmpty(iv))
            {
                return plainText;
            }
            try
            {
                using Aes aesAlg = Aes.Create();
                aesAlg.Key = Encoding.UTF8.GetBytes(key);
                aesAlg.IV = Encoding.UTF8.GetBytes(iv);
                var encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);
                using MemoryStream msEncrypt = new();
                using (CryptoStream cs = new(msEncrypt, encryptor, CryptoStreamMode.Write))
                using (StreamWriter sw = new(cs))
                {
                    sw.Write(plainText);
                }

                return Convert.ToBase64String(msEncrypt.ToArray());
            }
            catch (Exception ex)
            {
                return $"Error: {ex.Message}";
            }

        }

        /// <summary>
        /// Giải mã chuỗi đã mã hóa bằng AES với khoá và IV đã cho.
        /// </summary>
        /// <param name="cipherTextBase64"></param>
        /// <param name="key"></param>
        /// <param name="iv"></param>
        /// <returns></returns>
        public static string Decrypt(string cipherTextBase64, string key, string iv)
        {
            if (string.IsNullOrEmpty(cipherTextBase64) || string.IsNullOrEmpty(key) || string.IsNullOrEmpty(iv))
            {
                return cipherTextBase64;
            }
            try
            {
                using var aesAlg = Aes.Create();
                aesAlg.Key = Encoding.UTF8.GetBytes(key);
                aesAlg.IV = Encoding.UTF8.GetBytes(iv);

                var decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);
                using MemoryStream msDecrypt = new(Convert.FromBase64String(cipherTextBase64));
                using CryptoStream cs = new(msDecrypt, decryptor, CryptoStreamMode.Read);
                using StreamReader sr = new(cs);
                return sr.ReadToEnd();
            }
            catch (Exception ex)
            {
                return $"Error: {ex.Message}";
            }
        }

        /// <summary>
        /// Tạo chuỗi chữ ký HMAC SHA256 từ secret key và request body.
        /// </summary>
        /// <param name="secretkey"></param>
        /// <param name="requestBody"></param>
        /// <returns></returns>
        public static string ComputerHmacSHA256Signature(string secretkey, string requestBody)
        {
            using var hmac = new HMACSHA256(Encoding.UTF8.GetBytes(secretkey));
            var hashBytes = hmac.ComputeHash(Encoding.UTF8.GetBytes(requestBody));
            return BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
        }

        /// <summary>
        /// Tạo khoá Aes Key và Aes IV
        /// </summary>
        /// <returns></returns>
        public static string GenerateAes()
        {
            using var aes = Aes.Create();
            aes.GenerateKey();
            aes.GenerateIV();
            var key = Convert.ToBase64String(aes.Key);
            var iv = Convert.ToBase64String(aes.IV);
            return $"{key} - {iv}";
        }
    }
}
