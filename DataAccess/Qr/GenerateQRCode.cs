using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
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
        private readonly string _baseUrlWeb;
        public GenerateQRCode(IConfiguration configuration)
        {
            _baseUrlWeb = configuration["BaseUrlWeb"];
        }
        public string GenerateQRCodeForTable(int tableId)
        {
            string chatUrl = $"{_baseUrlWeb}/Shared/Order/OrderPage/{tableId}";

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
