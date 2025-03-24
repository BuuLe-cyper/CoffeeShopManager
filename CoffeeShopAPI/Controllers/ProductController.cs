using AutoMapper;
using BussinessObjects.DTOs;
using BussinessObjects.ImageService;
using BussinessObjects.Services;
using CoffeeShop.Helper;
using CoffeeShop.ViewModels;
using CoffeeShopAPI.Dtos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using System;
using System.Threading.Tasks;

namespace CoffeeShopAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;
        private readonly ICategoryService _categoryService;
        private readonly IImageService _imageService;
        private readonly IMapper _mapper;

        public ProductController(IProductService productService, ICategoryService categoryService, IImageService imageService, IMapper mapper)
        {
            _productService = productService;
            _categoryService = categoryService;
            _imageService = imageService;
            _mapper = mapper;
        }

        [HttpGet]
        [EnableQuery]

        public async Task<IActionResult> GetAllProducts()
        {
            var products = await _productService.GetAllProduct();
            if (products == null || !products.Any())
            {
                return NotFound("No products found.");
            }

            var productVMs = _mapper.Map<IEnumerable<ProductVM>>(products);
            return Ok(productVMs);
        }

        // Create Product
        [HttpPost]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> CreateProduct([FromForm] ProductCreateDto request)
        {
            if (request.File == null || request.File.Length == 0)
            {
                return BadRequest("Product image is required.");
            }

            // Validate file type
            bool isValidFile = CoffeeShop.Helper.Validations.IsImageFile(request.File);
            if (!isValidFile)
            {
                return BadRequest("Invalid image file. Allowed formats: JPG, PNG, JPEG, GIF.");
            }

            try
            {
                string imageUrl = await _imageService.UploadImage(request.File);
                request.ImageUrl = imageUrl;

                ProductDto productDto = _mapper.Map<ProductDto>(request);

                bool isAdded = await _productService.AddProduct(productDto);
                if (!isAdded)
                {
                    return BadRequest("Unable to create product. Product name might already exist.");
                }

                return Ok("Product created successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var product = await _productService.GetProduct(id);
            if (product == null)
            {
                return NotFound("Product not found.");
            }

            bool isDeleted = await _productService.SoftDeleteProduct(id);
            if (!isDeleted)
            {
                return BadRequest("Failed to delete product. Please try again.");
            }

            return Ok("Product deleted successfully.");
        }

        //[HttpPut("{id}")]
        //public async Task<IActionResult> UpdateProduct(int id, [FromForm] ProductVM productVM, [FromForm] IFormFile? file)
        //{
        //    if (id != productVM.ProductID)
        //    {
        //        return BadRequest("Product ID does not match.");
        //    }

        //    var existingProduct = await _productService.GetProduct(id);
        //    if (existingProduct == null)
        //    {
        //        return NotFound("Product not found.");
        //    }

        //    if (file != null)
        //    {
        //        bool checkFile = CoffeeShop.Helper.Validations.IsImageFile(file);
        //        if (!checkFile)
        //        {
        //            return BadRequest("Invalid image file. Allowed formats: JPG, PNG, JPEG, GIF.");
        //        }

        //        string imageUrl = await _imageService.UploadImage(file);
        //        productVM.ImageUrl = imageUrl;
        //    }
        //    else
        //    {
        //        productVM.ImageUrl = existingProduct.ImageUrl;
        //    }

        //    var updatedProductDto = _mapper.Map<ProductDto>(productVM);
        //    bool isUpdated = await _productService.UpdateProduct(updatedProductDto);

        //    if (!isUpdated)
        //    {
        //        return BadRequest("Unable to update product. Name might already exist.");
        //    }

        //    return Ok("Product updated successfully.");
        //}
    }

}
