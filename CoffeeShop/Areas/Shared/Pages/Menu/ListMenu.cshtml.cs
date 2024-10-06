using AutoMapper;
using BussinessObjects.Services;
using CoffeeShop.ViewModels;
using DataAccess.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace CoffeeShop.Areas.Shared.Pages.Menu
{
    public class ListMenuModel : PageModel
    {
        private readonly IProductSizesService _productSizesService;
        private readonly IMapper _mapper;
        public ListMenuModel(IProductSizesService productSizesService, IMapper mapper )
        {
            _productSizesService = productSizesService;
            _mapper = mapper;
        }
        public IEnumerable<ProductSizeVM> ProductSize { get; set; } = default!;

        public async Task OnGetAsync()
        {
            var result = await _productSizesService.GetAllProductSizes();
            ProductSize = _mapper.Map<IEnumerable<ProductSizeVM>>(result);
        }
    }
}
