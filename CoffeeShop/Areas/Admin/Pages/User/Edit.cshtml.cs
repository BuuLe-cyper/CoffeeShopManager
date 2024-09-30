using AutoMapper;
using BussinessObjects.DTOs;
using BussinessObjects.Services;
using CoffeeShop.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CoffeeShop.Areas.Admin.Pages.User
{
    [Authorize(Roles = "Admin")]
    public class EditModel : PageModel
    {
        private readonly IMapper _mapper;
        private readonly IUserService _service;

        public EditModel(IMapper mapper, IUserService service)
        {
            _mapper = mapper;
            _service = service;
        }

        [BindProperty]
        public new UserVM User { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(Guid? id)
        {
            var dtoUser = await _service.GetUser(id.Value);
            User = _mapper.Map<UserVM>(dtoUser);
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more information, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (ModelState.IsValid)
            {
                var dtoUser = _mapper.Map<UsersDTO>(User);
                await _service.UpdateUser(dtoUser);
            }
            return RedirectToPage("./Index");
        }
    }
}
