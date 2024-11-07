using AutoMapper;
using BussinessObjects.Services;
using CoffeeShop.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CoffeeShop.Areas.Shared.Pages.Product
{
    public class ListProductsModel : PageModel
    {
        private readonly IProductService _productService;
        private readonly IMapper _mapper;
        public ListProductsModel(IProductService productService, IMapper mapper)
        {
            _productService = productService;
            _mapper = mapper;
        }
        public IEnumerable<ProductVM> Product { get; set; } = default!;

        public async Task OnGetAsync()
        {
            var result = await _productService.GetAllProduct();
            Product = _mapper.Map<IEnumerable<ProductVM>>(result);
        }
    }
}
