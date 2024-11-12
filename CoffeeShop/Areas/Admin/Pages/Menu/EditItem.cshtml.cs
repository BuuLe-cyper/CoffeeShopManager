using AutoMapper;
using BussinessObjects.DTOs;
using BussinessObjects.Services;
using CoffeeShop.ViewModels;
using DataAccess.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;

namespace CoffeeShop.Areas.Admin.Pages.Menu
{
    public class EditItemModel : PageModel
    {
        private readonly IProductSizesService _productSizeService;
        private readonly IMapper _mapper;

        public EditItemModel(IProductSizesService productSizeService, IMapper mapper)
        {
            _productSizeService = productSizeService;
            _mapper = mapper;
        }

        [BindProperty]
        public ProductSizeVM ProductSize { get; set; } = new();
        [BindProperty(SupportsGet = true)]
        public int Id { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            if (Id == null)
            {
                return NotFound();
            }
            var productSize = await _productSizeService.GetProductSize((int)Id);
            if (productSize == null)
            {
                return NotFound();
            }
            ProductSize = _mapper.Map<ProductSizeVM>(productSize);

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (ProductSize.OriginalPrice > ProductSize.Price)
            {
                ModelState.AddModelError(string.Empty, "Original Price must be greater than or equal to Price.");
                return Page();
            }
            var productSizeEntity = await _productSizeService.GetProductSize((int)Id);
            productSizeEntity.Price = ProductSize.Price;
            productSizeEntity.OriginalPrice = ProductSize.OriginalPrice;
            var productSize = _mapper.Map<ProductSizeDto>(productSizeEntity);
            bool isSuccess = await _productSizeService.UpdateProductSize(productSize);

            if (!isSuccess)
            {
                ModelState.AddModelError(string.Empty, "There was an issue updating the product size.");
                return Page();
            }
            return RedirectToPage("/Menu/Index", new { area = "Admin" });

        }
    }
}