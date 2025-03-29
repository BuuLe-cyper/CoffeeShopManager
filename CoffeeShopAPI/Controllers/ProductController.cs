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
        private readonly IImageService _imageService;
        private readonly IMapper _mapper;

        public ProductController(IProductService productService, IImageService imageService, IMapper mapper)
        {
            _productService = productService;
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
    }

}
