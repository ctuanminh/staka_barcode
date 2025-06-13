using DevExpress.DataAccess.Native.Web;
using DevExpress.XtraScheduler.Outlook.Interop;
using FrmMain.Dto.Response;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Services;
using System;
using System.Globalization;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Windows.Forms;
using Exception = System.Exception;

namespace FrmMain
{
    public partial class FrmMain : DevExpress.XtraBars.Ribbon.RibbonForm
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _config;
        private readonly IHttpClientFactory _httpClientFactory;

        private const string OrderUrl = " https://public.kiotapi.com/orders/code/";
        public FrmMain(IHttpClientFactory httpClientFactory, HttpClient httpClient, IConfiguration config)
        {
            _httpClientFactory = httpClientFactory;
            _httpClient = httpClientFactory.CreateClient();
            _config = config;
            InitializeComponent();
        }

        private async void btnGetOrder_Click(object sender, EventArgs e)
        {
            try
            {
                var productCode = txtProductCode.Text;
                if (productCode == "")
                {
                    MessageBox.Show("Mã đơn hàng không được trống");
                    return;
                }
                var orderUrl = $"https://public.kiotapi.com/orders/code/{productCode}";

                using var client = new HttpClient();
                var token = await KiotVietAuthHelper.GetAccessTokenAsync(_httpClient, _config);
                client.BaseAddress = new Uri(orderUrl);
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                client.DefaultRequestHeaders.Add("Retailer", _config["KiotViet:Retailer"]);

                using var response = await client.GetAsync(orderUrl);

                var json = await response.Content.ReadAsStringAsync();
                if (response.IsSuccessStatusCode)
                {
                    var data = JsonConvert.DeserializeObject<OrderResponse>(json);
                    txtSale.Text = data.SoldByName;
                    txtBuyer.Text = data.CustomerName;
                    txtTotal.Text = data.Total.ToString(CultureInfo.InvariantCulture);
                    lblBranchName.Text = data.BranchName;
                    txtDiscount.Text = data.Discount.ToString();
                    txtKhachCanTra.Text = data.Total.ToString();
                    txtKhachThanhToan.Text = data.TotalPayment.ToString();
                    grdOrder.DataSource = data.OrderDetails;
                }
                else
                {
                    MessageBox.Show("Lỗi gọi API: " + response.StatusCode);
                }
            }
            catch (Exception exception)
            {
                MessageBox.Show("Lỗi gọi API: " + exception);
            }
        }

        private async void FrmMain_Load(object sender, EventArgs e)
        {
            try
            {
                const string orderUrl = $"https://public.kiotapi.com/orders";
                using var client = new HttpClient();
                var token = await KiotVietAuthHelper.GetAccessTokenAsync(_httpClient, _config);
                client.BaseAddress = new Uri(orderUrl);
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                client.DefaultRequestHeaders.Add("Retailer", _config["KiotViet:Retailer"]);

                using var response = await client.GetAsync(orderUrl);

                var json = await response.Content.ReadAsStringAsync();
            }
            catch (Exception exception)
            {
                MessageBox.Show("Lỗi gọi API: " + exception);
            }
        }
    }
}