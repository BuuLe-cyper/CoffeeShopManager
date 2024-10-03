using AutoMapper;
using BussinessObjects.Services;
using CoffeeShop.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CoffeeShop.Areas.Shared.Pages.Product
{
    public class DetailProduct : PageModel
    {
        private readonly IProductService _productService;
        private readonly IMapper _mapper;
        public DetailProduct(IProductService productService, IMapper mapper)
        {
            _productService = productService;
            _mapper = mapper;
        }

        public ProductVM Product { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            //var product = await _context.Products.FirstOrDefaultAsync(m => m.ProductID == id);
            var product = await _productService.GetProduct((int)id);
            if (product == null)
            {
                return NotFound();
            }
            else
            {
                Product = _mapper.Map<ProductVM>(product);
            }
            return Page();
        }
    }
}