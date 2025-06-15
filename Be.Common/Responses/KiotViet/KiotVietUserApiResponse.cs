using Be.Common.Dtos.CashFlow;
using System.Text.Json.Serialization;

namespace Be.Common.Responses.KiotViet
{
    public class KiotVietUserApiResponse
    {
        public List<KiotVietUserApi> Data { get; set; }
        public int Total { get; set; }
        public int PageSize { get; set; }
        public int CurrentItem { get; set; }
    }
    public class KiotVietUserApi
    {
        [JsonPropertyName("id")]
        public long Id { get; set; }

        [JsonPropertyName("userName")]
        public string UserName { get; set; }       

        [JsonPropertyName("givenName")]
        public string GivenName { get; set; }

        [JsonPropertyName("mobilePhone")]
        public string MobilePhone { get; set; }

        [JsonPropertyName("retailerId")]
        public long RetailerId { get; set; }

        [JsonPropertyName("email")]
        public string Email { get; set; }

        [JsonPropertyName("createdDate")]
        public DateTime CreatedDate { get; set; }
    }
}
