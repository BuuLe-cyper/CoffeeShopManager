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
        private readonly string _baseUrlApi;

        public IndexModel(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _baseUrlApi = configuration["BaseUrlApi"];
        }

        public List<TableVM> Tables { get; set; } = new List<TableVM>();

        public async Task OnGetAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync($"{_baseUrlApi}/api/Tables");
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
