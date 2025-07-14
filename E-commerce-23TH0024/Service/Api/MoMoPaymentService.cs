using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using Microsoft.Extensions.Configuration;

namespace E_commerce_23TH0024.Service.Api
{
    public class MoMoPaymentService
    {
        private readonly IConfiguration _configuration;

        public class MoMoResponse
        {
            public string PayUrl { get; set; }
            public string QrCodeUrl { get; set; }
        }

        public MoMoPaymentService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<MoMoResponse> GenerateMoMoQRCode(decimal amount, string orderId)
        {
            var config = _configuration.GetSection("MoMoSettings");
            var partnerCode = config["PartnerCode"];
            var accessKey = config["AccessKey"];
            var secretKey = config["SecretKey"];
            var endpoint = config["Endpoint"];
            var returnUrl = config["ReturnUrl"];
            var notifyUrl = config["NotifyUrl"];
            var requestId = Guid.NewGuid().ToString();
            var orderInfo = $"Thanh toán đơn hàng {orderId}";
            var extraData = ""; // Có thể thêm thông tin mã hóa nếu cần
            var requestType = "captureWallet";

            // Chuỗi raw để tạo chữ ký
            var rawHash = $"accessKey={accessKey}&amount={amount}&extraData={extraData}&ipnUrl={notifyUrl}&orderId={orderId}&orderInfo={orderInfo}&partnerCode={partnerCode}&redirectUrl={returnUrl}&requestId={requestId}&requestType={requestType}";
            var signature = GenerateSignature(rawHash, secretKey);

            var request = new
            {
                partnerCode,
                accessKey,
                requestId,
                amount = amount.ToString("F0"), // Không có phần thập phân
                orderId,
                orderInfo,
                redirectUrl = returnUrl,
                ipnUrl = notifyUrl,
                extraData,
                requestType,
                signature,
                lang = "vi"
            };

            using var client = new HttpClient();
            var response = await client.PostAsync(endpoint,
                new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json"));

            var responseString = await response.Content.ReadAsStringAsync();
            var json = JObject.Parse(responseString);

            return new MoMoResponse
            {
                PayUrl = json["payUrl"]?.ToString(),
                QrCodeUrl = json["qrCodeUrl"]?.ToString()
            };
        }

        private string GenerateSignature(string rawData, string secretKey)
        {
            var keyBytes = Encoding.UTF8.GetBytes(secretKey);
            var messageBytes = Encoding.UTF8.GetBytes(rawData);
            using var hmac = new HMACSHA256(keyBytes);
            var hash = hmac.ComputeHash(messageBytes);
            return BitConverter.ToString(hash).Replace("-", "").ToLower();
        }
    }
}
