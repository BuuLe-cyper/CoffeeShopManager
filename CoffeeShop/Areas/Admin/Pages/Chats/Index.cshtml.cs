using System.Net.Http;
using System.Text.Json;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using CoffeeShop.ViewModels.Tables;

namespace CoffeeShop.Areas.Admin.Pages.Chats
{
    [Authorize(Roles = "Admin")]
    public class IndexModel : PageModel
    {
        private readonly HttpClient _httpClient;

        public IndexModel(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public List<TableVM> Tables { get; set; } = new List<TableVM>();

        public async Task OnGetAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync("https://localhost:7158/api/Tables");
                if (!response.IsSuccessStatusCode)
                {
                    Tables = new List<TableVM>();
                    return;
                }


                var jsonResponse = await response.Content.ReadAsStringAsync();

                using var doc = JsonDocument.Parse(jsonResponse);


                Tables = JsonSerializer.Deserialize<List<TableVM>>(doc.RootElement.GetRawText(), new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });
            }
            catch
            {
                Tables = new List<TableVM>();
            }
        }
    }
}
