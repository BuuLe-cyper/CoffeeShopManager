using AutoMapper;
using BussinessObjects.Services;
using CoffeeShop.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CoffeeShop.Pages.User
{
    [Authorize(Roles ="Admin")]
    public class DeleteModel : PageModel
    {
        private readonly IMapper _mapper;
        private readonly IUserService _service;

        public DeleteModel(IMapper mapper, IUserService service)
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

        public async Task<IActionResult> OnPostAsync(Guid? id)
        {
            await _service.DeleteUser(id.Value);
            return RedirectToPage("./Index");
        }
    }
}
