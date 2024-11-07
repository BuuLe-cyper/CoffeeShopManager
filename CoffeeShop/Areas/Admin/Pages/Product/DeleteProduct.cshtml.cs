using AutoMapper;
using BussinessObjects.ImageService;
using BussinessObjects.Services;
using CoffeeShop.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace CoffeeShop.Areas.Admin.Pages.Product
{
    [Authorize(Roles = "Admin")]
    public class DeleteProduct (IServiceProvider serviceProvider) : PageModel
    {
        private readonly IProductService _productService = serviceProvider.GetRequiredService<IProductService>();
        private readonly ICategoryService _categoryService = serviceProvider.GetRequiredService<ICategoryService>();
        private readonly IImageService _imageService = serviceProvider.GetRequiredService<IImageService>();
        private readonly IMapper _mapper = serviceProvider.GetRequiredService<IMapper>();

        [BindProperty]
        public ProductVM Product { get; set; } = new ProductVM();

        [BindProperty]
        public IFormFile File { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _productService.GetProduct((int)id);
            if (product == null)
            {
                return NotFound();
            }
            Product = _mapper.Map<ProductVM>(product);
            ViewData["CategoryID"] = new SelectList(await _categoryService.GetAllCategory(), "CategoryID", "CategoryName");
            return Page();
        }
        public async Task<IActionResult> OnPostAsync()
        {

            try
            {
                int productID = Product.ProductID;
                var isUpdate = await _productService.SoftDeleteProduct(productID);
                if (!isUpdate)
                {
                    ViewData["CategoryID"] = new SelectList(await _categoryService.GetAllCategory(), "CategoryID", "CategoryName");
                    ModelState.AddModelError(string.Empty, "Unable to Delete because Name Exits. Please try again.");
                    return Page();
                }
            }
            catch (DbUpdateConcurrencyException)
            {
                throw;
            }

            return RedirectToPage("/Product/ListProducts", new { area = "Shared" });
        }
    }
}
