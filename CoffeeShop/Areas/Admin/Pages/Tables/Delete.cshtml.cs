using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using DataAccess.DataContext;
using DataAccess.Models;
using Microsoft.EntityFrameworkCore.Metadata;
using BussinessObjects.Services;
using AutoMapper;
using CoffeeShop.ViewModels.Tables;
using Microsoft.AspNetCore.Authorization;
using System.Text.Json;

namespace CoffeeShop.Areas.Admin.Pages.Tables
{
    [Authorize(Roles = "Admin")]
    public class DeleteModel : PageModel
    {
        private readonly HttpClient _httpClient;

        public DeleteModel(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        [BindProperty]
        public TableVM Table { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int id)
        {
            if (id <= 0)
            {
                return NotFound();
            }

            try
            {
                var response = await _httpClient.GetAsync($"https://localhost:7158/api/Tables/{id}");

                if (!response.IsSuccessStatusCode)
                {
                    return NotFound();
                }

                Table = JsonSerializer.Deserialize<TableVM>(
                    await response.Content.ReadAsStringAsync(),
                    new JsonSerializerOptions { PropertyNameCaseInsensitive = true }
                );

                return Page();
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, $"Error fetching table data: {ex.Message}");
                return Page();
            }
        }

        public async Task<IActionResult> OnPostAsync(int id)
        {
            if (id <= 0)
            {
                return NotFound();
            }

            try
            {
                var response = await _httpClient.DeleteAsync($"https://localhost:7158/api/Tables/{id}");

                if (!response.IsSuccessStatusCode)
                {
                    ModelState.AddModelError(string.Empty, "Error deleting table.");
                    return Page();
                }

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
