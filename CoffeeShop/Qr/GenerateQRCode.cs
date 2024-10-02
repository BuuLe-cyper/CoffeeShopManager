using QRCoder;
using System.Drawing.Imaging;
using System.Drawing;
using Microsoft.AspNetCore.Authorization;
using SkiaSharp;

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
            // Get the current request's scheme and host to build the URL dynamically
            var request = _httpContextAccessor.HttpContext.Request;
            string baseUrl = $"{request.Scheme}://{request.Host}";
            string chatUrl = $"{baseUrl}/Customer/Shopping/Index/{tableId}";

            try
            {
                using (QRCodeGenerator qrGenerator = new QRCodeGenerator())
                {
                    // Create QR code data
                    QRCodeData qrCodeData = qrGenerator.CreateQrCode(chatUrl, QRCodeGenerator.ECCLevel.Q);
                    int moduleCount = qrCodeData.ModuleMatrix.Count;

                    // Create a bitmap image of the QR code using SkiaSharp
                    using (var surface = SKSurface.Create(new SKImageInfo(moduleCount * 20, moduleCount * 20))) // 20 pixels per module
                    {
                        var canvas = surface.Canvas;
                        canvas.Clear(SKColors.White); // Set the background to white

                        // Draw the QR code
                        for (int x = 0; x < moduleCount; x++)
                        {
                            for (int y = 0; y < moduleCount; y++)
                            {
                                // Check if the module is black or white
                                if (qrCodeData.ModuleMatrix[x][y]) // Use ModuleMatrix to access module state
                                {
                                    canvas.DrawRect(new SKRect(x * 20, y * 20, (x + 1) * 20, (y + 1) * 20), new SKPaint { Color = SKColors.Black });
                                }
                            }
                        }

                        // Encode the image as PNG
                        using (var image = surface.Snapshot())
                        using (var data = image.Encode(SKEncodedImageFormat.Png, 100))
                        {
                            byte[] imageBytes = data.ToArray();
                            string base64String = Convert.ToBase64String(imageBytes);
                            return $"data:image/png;base64,{base64String}";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Log the exception here if needed
                throw new Exception($"Error generating QR code: {ex.Message}", ex);
            }
        }
    }
}
