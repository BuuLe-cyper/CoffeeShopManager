using BusinessObjects.Services;
using DataAccess.DataContext;
using DataAccess.Models;
using DataAccess.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace CoffeeShop.Pages.Users
{
    public class IndexModel : PageModel
    {
        private readonly IUserRepository _repository;
        private readonly MailService _mailService;

        public IndexModel(ApplicationDbContext context, MailService mailService)
        {
            _repository = new UserRepository(context);
            _mailService = mailService;
        }

        [Required(ErrorMessage = "UserName is Required")]
        [Display(Name ="Search UserName")]
        [BindProperty]
        public string Search { get; set; } = string.Empty;
        public IList<User> Users { get;set; } = default!;
        public async Task OnGetAsync()
        {
            Users = await _repository.FindAllUserAsync();

        }

        public async Task OnPostAsync()
        {
            var users = await _repository.FindAllUserAsync();
            Users = (from u in users
                    where u.Username.Contains(Search)
                    select u).ToList();
        }
    }
}
