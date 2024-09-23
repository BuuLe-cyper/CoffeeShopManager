using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using DataAccess.DataContext;
using DataAccess.Models;
using DataAccess.Repositories;

namespace CoffeeShop.Pages.Users
{
    public class CreateModel : PageModel
    {
        private readonly IUserRepository _repository;

        public CreateModel(ApplicationDbContext context)
        {
            _repository = new UserRepository(context);
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public new User User { get; set; } = default!;

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            await _repository.CreateAsync(User);

            return RedirectToPage("./Index");
        }
    }
}
