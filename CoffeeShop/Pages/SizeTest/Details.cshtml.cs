using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using DataAccess.DataContext;
using DataAccess.Models;

namespace CoffeeShop.Pages.SizeTest
{
    public class DetailsModel : PageModel
    {
        private readonly DataAccess.DataContext.ApplicationDbContext _context;

        public DetailsModel(DataAccess.DataContext.ApplicationDbContext context)
        {
            _context = context;
        }

        public Size Size { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var size = await _context.Sizes.FirstOrDefaultAsync(m => m.SizeID == id);
            if (size == null)
            {
                return NotFound();
            }
            else
            {
                Size = size;
            }
            return Page();
        }
    }
}
