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
using AutoMapper;
using BussinessObjects.Services;

namespace CoffeeShop.Pages.SizeTest
{
    public class EditModel : PageModel
    {
        private readonly ISizeService _sizeService;


        public EditModel(ISizeService sizeService)
        {
            _sizeService = sizeService;
        }

        [BindProperty]
        public Size Size { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var size = await _sizeService.GetSize((int)id);
            if (size == null)
            {
                return NotFound();
            }
            Size = size;
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more information, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            try
            {
                await _sizeService.UpdateSize(Size);    
            }
            catch (DbUpdateConcurrencyException)
            {
                    throw;
            }

            return RedirectToPage("./Index");
        }
    }
}
