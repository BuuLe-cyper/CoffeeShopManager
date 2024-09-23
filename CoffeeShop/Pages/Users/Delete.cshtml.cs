using DataAccess.Models;
using DataAccess.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace CoffeeShop.Pages.Users
{
    public class DeleteModel : PageModel
    {
        private readonly IUserRepository _repo;

        public DeleteModel(DataAccess.DataContext.ApplicationDbContext context)
        {
            _repo = new UserRepository(context);
        }

        [BindProperty]
        public new User User { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _repo.FindUserByID(id.Value);

            if (user == null)
            {
                return NotFound();
            }
            else
            {
                User = user;
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _repo.FindUserByID(id.Value);
            if (user != null)
            {
                User = user;
                await _repo.RemoveAsync(user);
            }

            return RedirectToPage("./Index"); 
        }
    }
}
