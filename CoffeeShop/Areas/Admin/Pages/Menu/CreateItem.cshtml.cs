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
        public ProductSizeVM ProductSize { get; set; } = default!;

        public async Task<IActionResult> OnPostAsync()
        {
            var item = _mapper.Map<ProductSizeDto>(ProductSize);
            bool isAdd = await _productSizesService.AddProductSize(item);
            if (!isAdd)
            {
                ModelState.AddModelError(string.Empty, "Unable to create size , category name existed. Please try again.");
                ViewData["ProductID"] = new SelectList(await _productService.GetAllProduct(), "ProductID", "ProductName");
                ViewData["SizeID"] = new SelectList(await _sizeService.GetAllSize(), "SizeID", "SizeName");
                return Page();
            }
            else
            {
                return Page();
            }
        }
    }
}

