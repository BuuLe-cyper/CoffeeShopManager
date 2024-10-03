using AutoMapper;
using BussinessObjects.DTOs;
using DataAccess.Models;
using DataAccess.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BussinessObjects.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;
        private readonly IMapper _mapper;
        public ProductService(IProductRepository productRepository, IMapper mapper)
        {
            _productRepository = productRepository;
            _mapper = mapper;
        }
        public async Task<bool> AddProduct(ProductDto productDto)
        {
            // three three fields must have value
            ArgumentNullException.ThrowIfNull(nameof(productDto.ProductName));
            ArgumentNullException.ThrowIfNull(nameof(productDto.CategoryID));
            ArgumentNullException.ThrowIfNull(nameof(productDto.Discount));

            try
            {
                productDto.ProductName = productDto.ProductName.Trim();
                var productNameExist = await GetProductName(productDto.ProductName);
                var deletedProductName = await _productRepository.GetAsync(item => item.ProductName == productDto.ProductName
                                                                          && item.IsDeleted == true && item.IsActive == false);
                // Product with same name but different category
                var productWithCategory = await _productRepository.GetAsync(item => item.ProductName == productDto.ProductName
                                                               && item.CategoryID == productDto.CategoryID
                                                               && item.IsDeleted == false);

                if (productNameExist == null || deletedProductName != null || productWithCategory == null)
                {
                    Product entity = _mapper.Map<Product>(productDto);
                    await _productRepository.CreateAsync(entity);
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred", ex);
            }
        }

        public async Task<IEnumerable<ProductViewDto>> GetAllProduct()
        {
            // If the category is deleted, then do not load the corresponding products
            var products = await _productRepository.GetAllAsync(p => p.IsDeleted == false && p.IsActive == true && p.IsAvailable == true
                                                                  && p.Category.IsDeleted == false && p.Category.IsActive == true
                                                                  , includeProperties: "Category");
            return _mapper.Map<IEnumerable<ProductViewDto>>(products);
        }

        public async Task<ProductViewDto> GetProduct(int productID)
        {
            var product = await _productRepository.GetAsync(p => p.ProductID == productID && p.IsDeleted == false && p.IsActive == true && p.IsAvailable == true, includeProperties: "Category");
            return _mapper.Map<ProductViewDto>(product);
        }

        public async Task<IEnumerable<ProductViewDto>> GetProductsByCategoryID(int categoryID)
        {
            var productByCateID = await GetAllProduct();
            return productByCateID.Where(p => p.CategoryID == categoryID);
        }

        public async Task<ProductViewDto> GetProductName(string productName)
        {
            var product = await _productRepository.GetAsync(p => p.ProductName == productName && p.IsDeleted == false && p.IsActive == true && p.IsAvailable == true, includeProperties: "Category");
            return _mapper.Map<ProductViewDto>(product);
        }

        public async Task<bool> SoftDeleteProduct(int sizeID)
        {
            return await _productRepository.SoftDeleteProductEntity(sizeID);
        }

        public async Task<bool> UpdateProduct(ProductDto productDto)
        {
            ArgumentNullException.ThrowIfNull(productDto, nameof(productDto));

            try
            {
                var productExists = await _productRepository.GetAsync(p => p.ProductID == productDto.ProductID && p.IsDeleted == false && p.IsActive == true)
                    ?? throw new Exception("Product Not Found");
                // Cannot update a product with the same name and category as another existing product
                var productWithCateExit = await _productRepository.GetAsync(p => p.ProductName == productDto.ProductName && p.CategoryID == productDto.CategoryID);
                if (productWithCateExit == null)
                {

                    Product entity = _mapper.Map<Product>(productDto);
                    // Ensuring no permission to modify this field
                    entity.IsActive = true;
                    entity.IsDeleted = false;
                    entity.CreateDate = productExists.CreateDate;
                    entity.ModifyDate = DateTime.Now;

                    await _productRepository.UpdateAsync(entity);
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred", ex);
            }
        }
    }
}