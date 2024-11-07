using AutoMapper;
using BussinessObjects.Services;
using CoffeeShop.ViewModels;
using DataAccess.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CoffeeShop.Areas.Shared.Pages.Order
{
    public class OrderPageModel : PageModel
    {
        private readonly IProductService _productService;
        private readonly ICategoryService _categoryService;
        private readonly IProductSizesService _productSizesService;
        private readonly IMapper _mapper;

        public OrderPageModel(ICategoryService categoryService, IProductService productService, IProductSizesService productSizesService, IMapper mapper)
        {
            _categoryService = categoryService;
            _productService = productService;
            _productSizesService = productSizesService;
            _mapper = mapper;
        }

        public IEnumerable<CategoryVM> Category { get; set; } = default!;
        public IEnumerable<ProductVM> Product { get; set; } = default!;
        public IEnumerable<ProductSizeVM> ProductSize { get; set; } = default!;

        public async Task OnGetAsync(int? productId, int? sizeId)
        {
            var categpry = await _categoryService.GetAllCategory();
            Category = _mapper.Map<IEnumerable<CategoryVM>>(categpry);

            var products = await _productService.GetAllProduct();
            Product = _mapper.Map<IEnumerable<ProductVM>>(products);

            var productSizes = await _productSizesService.GetAllProductSizes();
            ProductSize = _mapper.Map<IEnumerable<ProductSizeVM>>(productSizes);
        }
    }
}
