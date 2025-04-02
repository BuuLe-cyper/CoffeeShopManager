using CoffeeShop.ViewModels.Tables;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using System.Text;
namespace CoffeeShop.Areas.Admin.Pages.Tables
{
    public class CreateModel : PageModel
    {
        private readonly HttpClient _httpClient;
        private readonly string _baseUrlApi;

        public CreateModel(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _baseUrlApi = configuration["BaseUrlApi"];
        }

        [BindProperty]
        public string Description { get; set; }

        public TableVM Table { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return Page();
                }

                if (string.IsNullOrWhiteSpace(Description))
                {
                    return RedirectToPage("./Index");
                }

                var content = new StringContent(JsonSerializer.Serialize(Description), Encoding.UTF8, "application/json");
                var apiUrl = $"{_baseUrlApi}/api/Tables";
                var response = await _httpClient.PostAsync(apiUrl, content);

                if (!response.IsSuccessStatusCode)
                {
                    ModelState.AddModelError(string.Empty, "Error creating table.");
                    return Page();
                }

                Table = JsonSerializer.Deserialize<TableVM>(await response.Content.ReadAsStringAsync(), new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                return RedirectToPage("./Index");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, $"An error occurred: {ex.Message}");
                return Page();
            }
        }
    }
}
