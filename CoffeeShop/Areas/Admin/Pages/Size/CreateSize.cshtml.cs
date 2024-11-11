using AutoMapper;
using BussinessObjects.DTOs;
using BussinessObjects.Services;
using CoffeeShop.Helper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CoffeeShop.Areas.Admin.Pages.Size
{
    [Authorize(Roles = "Admin")]
    public class CreateSizeModel : PageModel
    {
        private readonly ISizeService _sizeService;
        private readonly IMapper _mapper;

        public CreateSizeModel(ISizeService sizeService, IMapper mapper)
        {
            _sizeService = sizeService;
            _mapper = mapper;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public DataAccess.Models.Size Size { get; set; } = default!;

        public async Task<IActionResult> OnPostAsync()
        {
            ArgumentNullException.ThrowIfNull(nameof(Size.SizeName));
            Size.SizeName = Size.SizeName.Trim();
            bool isValidData = Helper.Validations.IsString(Size.SizeName);
            if (isValidData)
            {
                SizeDto sizeDto = _mapper.Map<SizeDto>(Size);
                var isAdd = await _sizeService.AddSize(sizeDto);
                if (!isAdd)
                {
                    ModelState.AddModelError(string.Empty, "Unable to create size, size name existed. Please try again.");
                    return Page();
                }
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Size Name Must Be String. Please try again.");
                return Page();
            }
            return RedirectToPage("/Size/ListSizes", new { area = "Shared" });
        }
    }
}
