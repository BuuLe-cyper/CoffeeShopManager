using AutoMapper;
using BussinessObjects.Services;
using CoffeeShop.ViewModels;
using DataAccess.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Globalization;

namespace CoffeeShop.Areas.Shared.Pages.Order
{
    [AllowAnonymous]
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

        public string TableId { get; set; }
        public IEnumerable<CategoryVM> Category { get; set; } = default!;
        public IEnumerable<ProductVM> Product { get; set; } = default!;
        public IEnumerable<ProductSizeVM> ProductSize { get; set; } = default!;

        public async Task OnGetAsync(int? productId, int? sizeId, string Id, int? categoryId)
        {
            var categories = await _categoryService.GetAllCategory();
            Category = categories != null ? _mapper.Map<IEnumerable<CategoryVM>>(categories) : new List<CategoryVM>();

            var products = await _productService.GetAllProduct();
            Product = products != null ? _mapper.Map<IEnumerable<ProductVM>>(products) : new List<ProductVM>();

            if (categoryId.HasValue)
            {
                products = products.Where(p => p.CategoryID == categoryId.Value).ToList();
            }

            var productSizes = await _productSizesService.GetAllProductSizes();
            ProductSize = productSizes != null ? _mapper.Map<IEnumerable<ProductSizeVM>>(productSizes) : new List<ProductSizeVM>();

            foreach (var product in Product)
            {
                var productSize = ProductSize.FirstOrDefault(ps => ps.ProductID == product.ProductID);
                if (productSize != null)
                {
                    product.FormattedPrice = productSize.Price.ToString("N0", CultureInfo.InvariantCulture);
                }
            }

            TableId = Id;
        }
    }
}
