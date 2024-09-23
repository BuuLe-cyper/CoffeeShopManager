using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using DataAccess.DataContext;
using DataAccess.Models;
using DataAccess.Repositories;

namespace CoffeeShop.Pages.Users
{
    public class DetailsModel : PageModel
    {
        private readonly IUserRepository _rep;

        public DetailsModel(ApplicationDbContext context)
        {
            _rep = new UserRepository(context);
        }

        public new User User { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var user = await _rep.FindUserByID(id.Value);
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
    }
}
