using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using DataAccess.DataContext;
using DataAccess.Models;
using BussinessObjects.Services;
using AutoMapper;
using Firebase.Auth;
using Firebase.Storage;
using CoffeeShop.ViewModels;
using BussinessObjects.DTOs;
using CoffeeShop.Helper;
using BussinessObjects.ImageService;

namespace CoffeeShop.Pages.ProductTest
{
    public class CreateModel : PageModel
    {
        private readonly IProductService _productService;
        private readonly ICategoryService _categoryService;
        private readonly IMapper _mapper;
        private readonly IImageService _imageService;

        public CreateModel(IProductService productService, ICategoryService categoryService, IMapper mapper, IImageService imageService)
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
            var checkFile = Validations.IsImageFile(File);
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

            return RedirectToPage("./Index");
        }
    }
}