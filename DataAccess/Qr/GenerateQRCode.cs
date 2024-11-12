using Microsoft.AspNetCore.Http;
using QRCoder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Qr
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

            var request = _httpContextAccessor.HttpContext.Request;
            string baseUrl = $"{request.Scheme}://{request.Host}";
            string chatUrl = $"{baseUrl}/Shared/Order/OrderPage/{tableId}";

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
