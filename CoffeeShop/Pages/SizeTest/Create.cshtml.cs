using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using DataAccess.DataContext;
using DataAccess.Models;

namespace CoffeeShop.Pages.SizeTest
{
    public class CreateModel : PageModel
    {
        private readonly DataAccess.DataContext.ApplicationDbContext _context;

        public CreateModel(DataAccess.DataContext.ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public Size Size { get; set; } = default!;

        // For more information, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Sizes.Add(Size);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
