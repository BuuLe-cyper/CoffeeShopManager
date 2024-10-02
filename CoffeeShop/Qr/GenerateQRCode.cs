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
            return $"data:image/png;base64,";
            //// Get the current request's scheme and host to build the URL dynamically
            //var request = _httpContextAccessor.HttpContext.Request;
            //string baseUrl = $"{request.Scheme}://{request.Host}";
            //string chatUrl = $"{baseUrl}/Customer/Shopping/Index/{tableId}";

            //using (QRCodeGenerator qrGenerator = new QRCodeGenerator())
            //{
            //    QRCodeData qrCodeData = qrGenerator.CreateQrCode(chatUrl, QRCodeGenerator.ECCLevel.Q);
            //    using (QRCode qrCode = new QRCode(qrCodeData))
            //    {
            //        // Use the GetGraphic method from QRCoder
            //        using (Bitmap qrCodeImage = qrCode.GetGraphic(20))
            //        {
            //            using (MemoryStream ms = new MemoryStream())
            //            {
            //                qrCodeImage.Save(ms, ImageFormat.Png);
            //                byte[] imageBytes = ms.ToArray();
            //                string base64String = Convert.ToBase64String(imageBytes);
            //                return $"data:image/png;base64,{base64String}";
            //            }
            //        }
            //    }
            //}
        }
    }
}
