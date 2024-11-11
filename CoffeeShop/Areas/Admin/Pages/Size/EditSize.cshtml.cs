using AutoMapper;
using BussinessObjects.DTOs;
using BussinessObjects.Services;
using CoffeeShop.Helper;
using CoffeeShop.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace CoffeeShop.Areas.Admin.Pages.Size
{
    [Authorize(Roles = "Admin")]
    public class EditSizeModel : PageModel
    {
        private readonly ISizeService _sizeService;
        private readonly IMapper _mapper;

        public EditSizeModel(ISizeService sizeService, IMapper mapper)
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
            Size = _mapper.Map<SizeVM>(size);
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more information, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            ArgumentNullException.ThrowIfNull(nameof(Size.SizeName));
            Size.SizeName = Size.SizeName.ToUpper().Trim();
            bool isValidData = Helper.Validations.IsString(Size.SizeName);
            try
            {
                if (isValidData)
                {
                    var isUpdate = await _sizeService.UpdateSize(_mapper.Map<SizeViewDto>(Size));

                    if (!isUpdate)
                    {
                        ModelState.AddModelError(string.Empty, "Unable to Update because Name Exits. Please try again.");
                        return Page();
                    }
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Size Name Must Be String. Please try again.");
                    return Page();
                }
            }
            catch (DbUpdateConcurrencyException)
            {
                throw;
            }

            return RedirectToPage("/Size/ListSizes", new { area = "Shared" });
        }
    }
}
