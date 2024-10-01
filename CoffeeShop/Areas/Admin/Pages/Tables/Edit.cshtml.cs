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

namespace CoffeeShop.Areas.Admin.Pages.Tables
{
    [Authorize(Roles = "Admin")]
    public class EditModel : PageModel
    {
        private readonly ITableService _tableService;
        private readonly IMapper _mapper;
        public EditModel(ITableService tableService, IMapper mapper)
        {
            _tableService = tableService;
            _mapper = mapper;
        }

        [BindProperty]
        public TableVM Table { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var table = _mapper.Map<TableVM>(await _tableService.GetAsync(id));

            if (table == null)
            {
                return NotFound();
            }
            Table = table;
            return Page();
        }
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            Table.ModifyDate = DateTime.Now;
            await _tableService.UpdateTableAsync(_mapper.Map<TableDTO>(Table));

            return RedirectToPage("./Index");
        }
    }
}
