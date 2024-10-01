using Microsoft.AspNetCore.Http;
using QRCoder;
using System.Drawing;
using System.Drawing.Imaging;

namespace DataAccess.QR
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
            // Get the current request's scheme and host to build the URL dynamically
            var request = _httpContextAccessor.HttpContext.Request;
            string baseUrl = $"{request.Scheme}://{request.Host}";
            string chatUrl = $"{baseUrl}/Customer/Shopping/Index/{tableId}";

            using (QRCodeGenerator qrGenerator = new QRCodeGenerator())
            {
                QRCodeData qrCodeData = qrGenerator.CreateQrCode(chatUrl, QRCodeGenerator.ECCLevel.Q);
                using (QRCode qrCode = new QRCode(qrCodeData))
                {
                    using (Bitmap qrCodeImage = qrCode.GetGraphic(20))
                    {
                        using (MemoryStream ms = new MemoryStream())
                        {
                            qrCodeImage.Save(ms, ImageFormat.Png);
                            byte[] imageBytes = ms.ToArray();
                            string base64String = Convert.ToBase64String(imageBytes);
                            return $"data:image/png;base64,{base64String}";
                        }
                    }
                }
            }
        }
    }
}
