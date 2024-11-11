using AutoMapper;
using BussinessObjects.Services;
using CoffeeShop.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CoffeeShop.Areas.Admin.Pages.Size
{
    [Authorize(Roles = "Admin")]
    public class DeleteSizeModel : PageModel
    {
        private readonly ISizeService _sizeService;
        private readonly IMapper _mapper;
        public DeleteSizeModel(ISizeService sizeService, IMapper mapper)
        {
            _sizeService = sizeService;
            _mapper = mapper;
        }
        [BindProperty]
        public SizeVM Size { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var size = await _sizeService.GetSize((int)id);

            if (size == null)
            {
                return NotFound();
            }
            else
            {
                Size = _mapper.Map<SizeVM>(size);
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var size = await _sizeService.GetSize((int)id);
            if (size != null)
            {
                Size = _mapper.Map<SizeVM>(size);
                var isRemove = await _sizeService.SoftDeleteSize(size.SizeID);
                if (!isRemove)
                {
                    ModelState.AddModelError(string.Empty, "Unable to delete size. Please try again.");
                    return Page();
                }
            }
            return RedirectToPage("/Size/ListSizes", new { area = "Shared" });
        }
    }
}
