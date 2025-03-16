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
                    pageIndex = 1; // Nếu tìm kiếm mới, quay về trang đầu
                }
                else
                {
                    searchString = currentFilter; // Giữ bộ lọc trước đó nếu không nhập mới
                }

                CurrentFilter = searchString;

                var pageSize = 5;
                var skip = ((pageIndex ?? 1) - 1) * pageSize;
                var odataQuery = $"?$orderby=TableID&$skip={skip}&$top={pageSize}&$count=true";

                if (!string.IsNullOrEmpty(searchString))
                {
                    odataQuery = $"?$filter=contains(Description,'{searchString}')&$orderby=TableID&$skip={skip}&$top={pageSize}&$count=true";
                }

                var response = await _httpClient.GetAsync($"https://your-api-url/api/Tables{odataQuery}");

                if (!response.IsSuccessStatusCode)
                {
                    ModelState.AddModelError(string.Empty, "Error fetching table data.");
                    return;
                }

                var jsonResponse = await response.Content.ReadAsStringAsync();
                var odataResponse = JsonSerializer.Deserialize<ODataResponse<TableVM>>(jsonResponse, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                Table = odataResponse.Value;
                PageIndex = pageIndex ?? 1;
                TotalPages = (int)Math.Ceiling((double)odataResponse.Count / pageSize);
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
                var response = await _httpClient.GetAsync($"https://your-api-url/api/Tables/{id}/QRCode");

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

    public class ODataResponse<T>
    {
        public List<T> Value { get; set; }
        public int Count { get; set; }
    }
}
