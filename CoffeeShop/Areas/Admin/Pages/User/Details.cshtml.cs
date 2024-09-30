using AutoMapper;
using BussinessObjects.Services;
using CoffeeShop.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CoffeeShop.Areas.Admin.Pages.User
{
    [Authorize(Roles = "Admin")]
    public class DetailsModel : PageModel
    {
        private readonly IUserService _service;
        private readonly IMapper _mapper;

        public DetailsModel(IUserService service, IMapper mapper)
        {
            _service = service;
            _mapper = mapper;
        }

        public new UserVM User { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(Guid? id)
        {
            var dtoUser = await _service.GetUser(id.Value);
            if (dtoUser == null) return NotFound();
            User = _mapper.Map<UserVM>(dtoUser);
            return Page();
        }
    }
}
