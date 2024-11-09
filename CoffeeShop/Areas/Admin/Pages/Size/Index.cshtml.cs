using AutoMapper;
using BussinessObjects.Services;
using CoffeeShop.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CoffeeShop.Areas.Admin.Pages.Size
{
    public class IndexModel : PageModel
    {
        private readonly ISizeService _sizeService;
        private readonly IMapper _mapper;
        public IndexModel(ISizeService sizeService, IMapper mapper)
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
