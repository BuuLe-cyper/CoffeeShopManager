using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using CoffeeShop.ViewModels.Tables;

namespace CoffeeShop.Areas.Admin.Pages.Tables
{
    [Authorize(Roles = "Admin")]
    public class IndexModel : PageModel
    {
        private readonly HttpClient _httpClient;

        public IndexModel(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public string CurrentFilter { get; set; }
        public int PageIndex { get; set; }
        public int TotalPages { get; set; }
        public List<TableVM> Table { get; set; } = new List<TableVM>();

        public async Task OnGetAsync(string currentFilter, string searchString, int? pageIndex)
        {
            try
            {
                if (!string.IsNullOrEmpty(searchString))
                {
                    pageIndex = 1;
                }
                else
                {
                    searchString = currentFilter;
                }

                CurrentFilter = searchString;

                var apiBaseUrl = "https://localhost:7158";
                var query = "?$orderby=CreateDate desc";
                if (!string.IsNullOrEmpty(searchString))
                {
                    var escapedSearch = Uri.EscapeDataString(searchString);
                    query += $"&$filter=contains(Description,'{escapedSearch}')";
                }
                var fullUrl = $"{apiBaseUrl}/api/Tables{query}";
                Console.WriteLine(fullUrl);

                var response = await _httpClient.GetAsync($"{apiBaseUrl}/api/Tables{query}");

                if (!response.IsSuccessStatusCode)
                {
                    ModelState.AddModelError(string.Empty, "Error fetching table data.");
                    return;
                }

                var jsonResponse = await response.Content.ReadAsStringAsync();

                using var doc = JsonDocument.Parse(jsonResponse);

                var valuesElement = doc.RootElement.GetProperty("$values");

                var allTables = JsonSerializer.Deserialize<List<TableVM>>(valuesElement.GetRawText(), new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                PageIndex = pageIndex ?? 1;
                int pageSize = 5;
                TotalPages = (int)Math.Ceiling((double)allTables.Count / pageSize);

                Table = allTables
                    .Skip((PageIndex - 1) * pageSize)
                    .Take(pageSize)
                    .ToList();
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, $"An error occurred: {ex.Message}");
            }
        }

        public async Task<IActionResult> OnGetDownloadQRCode(int id)
        {
            try
            {
                var response = await _httpClient.GetAsync($"https://localhost:7158/api/Tables/{id}/QRCode");

                if (!response.IsSuccessStatusCode)
                {
                    return NotFound();
                }

                var imageBytes = await response.Content.ReadAsByteArrayAsync();
                return File(imageBytes, "image/png", $"QRCode_{id}.png");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, $"An error occurred: {ex.Message}");
                return Page();
            }
        }
    }
}
