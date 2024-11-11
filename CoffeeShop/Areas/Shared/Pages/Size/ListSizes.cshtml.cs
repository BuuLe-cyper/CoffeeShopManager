using AutoMapper;
using BussinessObjects.Services;
using CoffeeShop.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CoffeeShop.Areas.Shared.Pages.Size
{
    public class ListSizesModel : PageModel
    {
        private readonly ISizeService _sizeService;
        private readonly IMapper _mapper;
        public ListSizesModel(ISizeService sizeService, IMapper mapper)
        {
            _sizeService = sizeService;
            _mapper = mapper;
        }

        public IEnumerable<SizeVM> Size { get; set; } = default!;

        public async Task OnGetAsync()
        {
            var result = await _sizeService.GetAllSize();
            Size = _mapper.Map<IEnumerable<SizeVM>>(result);
        }
    }
}
