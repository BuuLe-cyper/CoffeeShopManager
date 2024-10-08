using AutoMapper;
using BussinessObjects.DTOs;
using BussinessObjects.Services;
using CoffeeShop.ViewModels;
using DataAccess.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CoffeeShop.Areas.Admin.Pages.Menu
{
    [Authorize(Roles = "Admin")]
    public class CreateItemModel : PageModel
    {
        private readonly IProductSizesService _productSizesService;
        private readonly IProductService _productService;
        private readonly ISizeService _sizeService;
        private readonly IMapper _mapper;

        public CreateItemModel(IProductSizesService productSizesService, IMapper mapper, IProductService productService, ISizeService sizeService)
        {
            _productSizesService = productSizesService;
            _mapper = mapper;
            _productService = productService;
            _sizeService = sizeService;
        }

        public async Task<IActionResult> OnGet()
        {
            ViewData["ProductID"] = new SelectList(await _productService.GetAllProduct(), "ProductID", "ProductName");
            ViewData["SizeID"] = new SelectList(await _sizeService.GetAllSize(), "SizeID", "SizeName");
            return Page();
        }

        [BindProperty]
        public MenuItemVMDto ProductSize { get; set; } = default!;
        public async Task<IActionResult> OnPostAsync()
        {
            // Handle list SizePrices
            bool isAnySizeAdded = false;
            foreach (var itemSizePrice in ProductSize.SizePrices)
            {
                if (itemSizePrice.Price > 0)
                {
                    ProductSizeVM productSize = _mapper.Map<ProductSizeVM>(ProductSize);
                    productSize.SizeID = itemSizePrice.SizeID;
                    productSize.Price = itemSizePrice.Price;
                    var item = _mapper.Map<ProductSizeDto>(productSize);
                    bool isAdd = await _productSizesService.AddProductSize(item);
                    if (isAdd)
                    {
                        isAnySizeAdded = true;
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, $"Unable to create size for SizeID: {itemSizePrice.SizeID}. Please try again.");
                        ViewData["ProductID"] = new SelectList(await _productService.GetAllProduct(), "ProductID", "ProductName");
                        ViewData["SizeID"] = new SelectList(await _sizeService.GetAllSize(), "SizeID", "SizeName");
                        return Page();
                    }
                }
            }
            if (!isAnySizeAdded)
            {
                ModelState.AddModelError(string.Empty, "No sizes were added successfully. Please check the size prices and try again.");
                ViewData["ProductID"] = new SelectList(await _productService.GetAllProduct(), "ProductID", "ProductName");
                ViewData["SizeID"] = new SelectList(await _sizeService.GetAllSize(), "SizeID", "SizeName");
                return Page();
            }
            return RedirectToPage("/Menu/ListMenu", new { area = "Shared" });
        }
    }
}

