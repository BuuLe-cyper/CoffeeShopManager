using AutoMapper;
using BussinessObjects.Services;
using CoffeeShop.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CoffeeShop.Areas.Shared.Pages.Size
{
    public class DetailSizeModel : PageModel
    {
        private readonly ISizeService _sizeService;
        private readonly IMapper _mapper;

        public DetailSizeModel(ISizeService sizeService, IMapper mapper)
        {
            _sizeService = sizeService;
            _mapper = mapper;
        }

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
    }
}
