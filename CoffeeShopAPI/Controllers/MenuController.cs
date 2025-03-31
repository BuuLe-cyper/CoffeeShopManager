using AutoMapper;
using BussinessObjects.DTOs;
using BussinessObjects.Services;
using CoffeeShop.Helper;
using CoffeeShop.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CoffeeShopAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MenuController : ControllerBase
    {
        private readonly IProductSizesService _productSizesService;
        private readonly IMapper _mapper;

        public MenuController(IProductSizesService productSizesService, IMapper mapper)
        {
            _productSizesService = productSizesService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllMenus(
            [FromQuery] string? searchQuery,
            [FromQuery] int pageIndex = 1,
            [FromQuery] int pageSize = 4)
        {
            var result = await _productSizesService.GetAllProductSizes();
            if (result == null || !result.Any()) return NotFound("No menu items found.");

            var filteredResults = result
                .Where(x => !x.Size.IsDeleted &&
                            (string.IsNullOrEmpty(searchQuery) ||
                             x.Product.ProductName.Contains(searchQuery, StringComparison.OrdinalIgnoreCase) ||
                             x.Size.SizeName.Contains(searchQuery, StringComparison.OrdinalIgnoreCase)))
                .AsQueryable();

            var groupedResults = filteredResults.GroupBy(x => x.ProductID).ToList();
            var count = groupedResults.Count;
            var pageGroups = groupedResults.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
            var items = pageGroups.SelectMany(g => g.Select(x => _mapper.Map<ProductSizeVM>(x))).ToList();

            return Ok(new PaginatedList<ProductSizeVM>(items, count, pageIndex, pageSize));
        }

        [HttpPost("CreateItem")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateMenuItem([FromBody] MenuItemVMDto productSizeDto)
        {
            if (productSizeDto == null || productSizeDto.SizePrices == null || !productSizeDto.SizePrices.Any())
            {
                return BadRequest("Invalid menu item data.");
            }

            bool isAnySizeAdded = false;

            foreach (var itemSizePrice in productSizeDto.SizePrices)
            {
                if (itemSizePrice.Price <= 0 || itemSizePrice.OriginalPrice <= 0 || productSizeDto.ProductID == 0)
                {
                    continue;
                }

                var productSize = _mapper.Map<ProductSizeVM>(productSizeDto);
                productSize.Price = itemSizePrice.Price;
                productSize.OriginalPrice = itemSizePrice.OriginalPrice;
                productSize.SizeID = itemSizePrice.SizeID;
                productSize.ProductID = productSizeDto.ProductID;

                var item = _mapper.Map<ProductSizeDto>(productSize);
                bool isAdded = await _productSizesService.AddProductSize(item);

                if (isAdded)
                {
                    isAnySizeAdded = true;
                }
                else
                {
                    return BadRequest($"Failed to add size for SizeID: {itemSizePrice.SizeID}. Please try again.");
                }
            }

            if (!isAnySizeAdded)
            {
                return BadRequest("No sizes were added successfully. Please check the size prices and try again.");
            }

            return Ok("Menu item created successfully.");
        }

        [HttpPut("{EditItem}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> EditMenuItem(int id, [FromBody] ProductSizeVM productSizeVM)
        {
            if (productSizeVM == null || id != productSizeVM.ProductID)
            {
                return BadRequest("Invalid data.");
            }

            if (productSizeVM.OriginalPrice < productSizeVM.Price)
            {
                return BadRequest("Original Price must be greater than or equal to Price.");
            }

            var existingProductSize = await _productSizesService.GetProductSize(id);
            if (existingProductSize == null)
            {
                return NotFound("Product size not found.");
            }

            existingProductSize.Price = productSizeVM.Price;
            existingProductSize.OriginalPrice = productSizeVM.OriginalPrice;

            var productSizeDto = _mapper.Map<ProductSizeDto>(existingProductSize);
            bool isSuccess = await _productSizesService.UpdateProductSize(productSizeDto);

            if (!isSuccess)
            {
                return BadRequest("There was an issue updating the product size.");
            }

            return Ok("Product size updated successfully.");
        }
    }
}
