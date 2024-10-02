using QRCoder;
using System.Drawing.Imaging;
using System.Drawing;

namespace CoffeeShop.Qr
{
    public class GenerateQRCode
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        public GenerateQRCode(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public string GenerateQRCodeForTable(int tableId)
        {

            //// Get the current request's scheme and host to build the URL dynamically
            var request = _httpContextAccessor.HttpContext.Request;
            string baseUrl = $"{request.Scheme}://{request.Host}";
            string chatUrl = $"{baseUrl}/Customer/Shopping/Index/{tableId}";

            using (QRCodeGenerator qrGenerator = new QRCodeGenerator())
            {
                QRCodeData qrCodeData = qrGenerator.CreateQrCode(chatUrl, QRCodeGenerator.ECCLevel.Q);
               
                using (Base64QRCode qrCode = new Base64QRCode(qrCodeData))
                {
                    // Use the GetGraphic method from QRCoder
                    string qrCodeImageAsBase64 = qrCode.GetGraphic(20);
                    return $"data:image/png;base64,{qrCodeImageAsBase64}";
                }
            }
        }
    }
}
