using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using DataAccess.DataContext;
using DataAccess.Models;
using CoffeeShop.ViewModels;
using AutoMapper;
using BussinessObjects.DTOs;
using BussinessObjects.ImageService;
using BussinessObjects.Services;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CoffeeShop.Pages.ProductTest
{
    public class DeleteModel(IServiceProvider serviceProvider) : PageModel
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

            return RedirectToPage("./Index");
        }
    }
}
