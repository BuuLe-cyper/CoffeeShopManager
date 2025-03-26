using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DataAccess.DataContext;
using DataAccess.Models;
using Microsoft.EntityFrameworkCore.Metadata;
using BussinessObjects.Services;
using AutoMapper;
using CoffeeShop.ViewModels.Tables;
using BussinessObjects.DTOs.Tables;
using Microsoft.AspNetCore.Authorization;
using System.Text.Json;
using System.Text;

namespace CoffeeShop.Areas.Admin.Pages.Tables
{
    [Authorize(Roles = "Admin")]
    public class EditModel : PageModel
    {
        private readonly HttpClient _httpClient;
        private readonly IMapper _mapper;

        public EditModel(HttpClient httpClient, IMapper mapper)
        {
            _httpClient = httpClient;
            _mapper = mapper;
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
                ModelState.AddModelError(string.Empty, $"Error fetching table details: {ex.Message}");
                return Page();
            }
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            try
            {
                Table.ModifyDate = DateTime.Now;

                var tableDto = _mapper.Map<TableDTO>(Table);


                var content = new StringContent(JsonSerializer.Serialize(tableDto), Encoding.UTF8, "application/json");
                var response = await _httpClient.PutAsync($"https://localhost:7158/api/Tables", content);

                if (!response.IsSuccessStatusCode)
                {
                    ModelState.AddModelError(string.Empty, "Error updating table.");
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
