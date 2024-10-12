using AutoMapper;
using BussinessObjects.DTOs;
using BussinessObjects.Services;
using CoffeeShop.ViewModels;
using DataAccess.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis;
using System.Collections;

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
        [BindProperty]
        public MenuItemVMDto ProductSize { get; set; } = default!;
        [BindProperty(SupportsGet = true)]
        public int? ProductID { get; set; }
        public async Task<IActionResult> OnGet()
        {
            IEnumerable<SizeViewDto> size = await _sizeService.GetAllSize();
            IEnumerable<ProductViewDto> product = await _productService.GetAllProduct();
            var allSizeProduct = await _productSizesService.GetAllProductSizes();
            // Filter 
            var validProductIds = allSizeProduct
                .GroupBy(it => it.ProductID)
                .Where(g => g.Select(gs => gs.SizeID).Distinct().Count() >= size.Count())
                .Select(g => g.Key)
                .ToList();

            if (ProductID.HasValue)
            {
                var sizeOfProductID = allSizeProduct
                    .Where(it => it.ProductID == ProductID)
                    .Select(it => it.SizeID)
                    .ToList();
                size = size.Where(s => !sizeOfProductID.Contains(s.SizeID)).ToList();

            }
            product = product.Where(p => !validProductIds.Contains(p.ProductID)).ToList();
            ViewData["ProductID"] = new SelectList(product, "ProductID", "ProductName");
            ViewData["SizeID"] = new SelectList(size, "SizeID", "SizeName");
            return Page();
        }
        public async Task<IActionResult> OnPostAsync()
        {
            // Handle list SizePrices
            bool isAnySizeAdded = false;
            foreach (var itemSizePrice in ProductSize.SizePrices)
            {
                if (itemSizePrice.Price > 0 && itemSizePrice.OriginalPrice > 0 && ProductID != 0)
                {
                    ProductSizeVM productSize = _mapper.Map<ProductSizeVM>(ProductSize);
                    productSize.Price = itemSizePrice.Price;
                    productSize.OriginalPrice = itemSizePrice.OriginalPrice;
                    productSize.SizeID = itemSizePrice.SizeID;
                    productSize.ProductID = (int)ProductID;
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

