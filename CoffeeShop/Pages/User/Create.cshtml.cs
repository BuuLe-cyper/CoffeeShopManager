using AutoMapper;
using BussinessObjects.DTOs;
using BussinessObjects.Services;
using CoffeeShop.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CoffeeShop.Pages.User
{
    [Authorize(Roles = "Admin")]
    public class CreateModel : PageModel
    {
        private readonly IMapper _mapper;
        private readonly IUserService _service;

        public CreateModel(IMapper mapper, IUserService service)
        {
            _mapper = mapper;
            _service = service;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public new UserVM User { get; set; } = default!;

        // For more information, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if(ModelState.IsValid)
            {
                var dtoUser = _mapper.Map<UsersDTO>(User);
                await _service.AddUser(dtoUser);
            }
            return RedirectToPage("./Index");
        }
    }
}
