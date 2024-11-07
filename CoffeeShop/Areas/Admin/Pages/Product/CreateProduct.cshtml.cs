using AutoMapper;
using BussinessObjects.DTOs;
using BussinessObjects.ImageService;
using BussinessObjects.Services;
using CoffeeShop.Helper;
using CoffeeShop.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CoffeeShop.Areas.Admin.Pages.Product
{
    [Authorize(Roles = "Admin")]
    public class CreateProductModel : PageModel
    {
        private readonly IProductService _productService;
        private readonly ICategoryService _categoryService;
        private readonly IMapper _mapper;
        private readonly IImageService _imageService;

        public CreateProductModel(IProductService productService, ICategoryService categoryService, IMapper mapper, IImageService imageService)
        {
            _productService = productService;
            _categoryService = categoryService;
            _mapper = mapper;
            _imageService = imageService;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            ViewData["CategoryID"] = new SelectList(await _categoryService.GetAllCategory(), "CategoryID", "CategoryName");
            return Page();
        }

        [BindProperty]
        public ProductVM Product { get; set; } = new ProductVM();
        [BindProperty]
        public IFormFile File { get; set; } = default!;

        public async Task<IActionResult> OnPostAsync()
        {
            var checkFile = CoffeeShop.Helper.Validations.IsImageFile(File);
            if (!checkFile)
            {
                throw new InvalidOperationException("File Image Not Valid");
            }
            try
            {
                string imageUrl = await _imageService.UploadImage(File);
                Product.ImageUrl = imageUrl;

                ProductDto productDto = _mapper.Map<ProductDto>(Product);
                bool isAdd = await _productService.AddProduct(productDto);
                if (!isAdd)
                {
                    ModelState.AddModelError(string.Empty, "Unable to create Product, product name existed. Please try again.");
                    ViewData["CategoryID"] = new SelectList(await _categoryService.GetAllCategory(), "CategoryID", "CategoryName", Product.CategoryID);
                    return Page();
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, "An error occurred while processing your request check file name JPG, PNG, JPEG, GIF Only.");
                ViewData["CategoryID"] = new SelectList(await _categoryService.GetAllCategory(), "CategoryID", "CategoryName", Product.CategoryID);
                return Page();
            }
            return RedirectToPage("/Product/ListProducts", new { area = "Shared" });
        }
    }
}