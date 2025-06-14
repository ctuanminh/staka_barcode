using FrmMain.Dto.Request;
using FrmMain.Dto.Response;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Services;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Windows.Forms;

namespace FrmMain
{
    public partial class FrmOrder : DevExpress.XtraEditors.XtraForm
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _config;
        private readonly IHttpClientFactory _httpClientFactory;

        private const string OrderUrl = " https://public.kiotapi.com/orders/code/";
        public FrmOrder()
        {
            InitializeComponent();
        }
        public FrmOrder(IHttpClientFactory httpClientFactory, HttpClient httpClient, IConfiguration config)
        {
            _httpClientFactory = httpClientFactory;
            _httpClient = _httpClientFactory.CreateClient();
            _config = config;
            InitializeComponent();
        }

        private async void FrmOrder_LoadAsync(object sender, EventArgs e)
        {

        }

        private async void FrmOrder_Activated(object sender, EventArgs e)
        {
            
        }

        private async void FrmOrder_Shown(object sender, EventArgs e)
        {
            try
            {
                var orderUrl = $"https://public.kiotapi.com/orders";

                using var client = new HttpClient();
                var token = await KiotVietAuthHelper.GetAccessTokenAsync(_httpClient, _config);
                client.BaseAddress = new Uri(orderUrl);
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                client.DefaultRequestHeaders.Add("Retailer", _config["KiotViet:Retailer"]);

                var request = new SearchOrderRequest()
                {
                    BranchIds = [631782],
                    Status = [1],
                };

                var (success, content) = await KiotVietApiHelper.CallApiAsync(_httpClient, _config, orderUrl, request);

                if (success && content != null)
                {
                    var data = JsonConvert.DeserializeObject<OrderResponse>(content);
                    grdControlOrders.DataSource = data;
                }
            }
            catch (Exception exception)
            {
                MessageBox.Show("Lỗi gọi API: " + exception);
            }
        }
    }
}