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
    public class ProductSizesService : IProductSizesService
    {
        private readonly IProductSizesRepository _productSizesRepository;
        private readonly IProductRepository _productRepository;
        private readonly ISizeRepository _sizeRepository;
        private readonly IMapper _mapper;
        public ProductSizesService(IProductSizesRepository productSizesRepository, IProductRepository productRepository, ISizeRepository sizeRepository, IMapper mapper)
        {
            _productSizesRepository = productSizesRepository;
            _productRepository = productRepository;
            _sizeRepository = sizeRepository;
            _mapper = mapper;
        }
        public async Task<bool> AddProductSize(ProductSizeDto productSizeDto)
        {
            ArgumentNullException.ThrowIfNull(nameof(productSizeDto));
            ArgumentNullException.ThrowIfNull(nameof(productSizeDto.ProductID));
            ArgumentNullException.ThrowIfNull(nameof(productSizeDto.SizeID));
            // Check Value for Price và OriginalPrice
            if (productSizeDto.Price < 0)
            {
                throw new ArgumentException("Price cannot be negative.", nameof(productSizeDto.Price));
            }

            if (productSizeDto.OriginalPrice < 0)
            {
                throw new ArgumentException("OriginalPrice cannot be negative.", nameof(productSizeDto.OriginalPrice));
            }
            try
            {
                var productExist = await _productRepository.GetAsync(p => p.ProductID == productSizeDto.ProductID);
                var sizeExist = await _sizeRepository.GetAsync(s => s.SizeID == productSizeDto.SizeID);
                var productSizeExist = await _productSizesRepository.GetAsync(item => item.ProductSizeID == productSizeDto.ProductSizeID && item.IsDeleted == false && item.IsActive == true);
                // Do not create if two item have the same product and size
                // Do not create if the has current productsize
                if ((productExist != null && sizeExist != null) || sizeExist != null)
                {
                    return false;
                }
                else
                {
                    ProductSize productSize = _mapper.Map<ProductSize>(productSizeDto);
                    await _productSizesRepository.CreateAsync(productSize);

                    return true;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IEnumerable<ProductSizesViewDto>> GetAllProductSizes()
        {
            var results = await _productSizesRepository.GetAllAsync(item => item.IsDeleted == false && item.IsActive == true, includeProperties: "Category,Product");
            return _mapper.Map<IEnumerable<ProductSizesViewDto>>(results);
        }

        public async Task<ProductSizesViewDto> GetProductSize(int productID)
        {
            var result = await _productSizesRepository.GetAsync(item => item.IsDeleted == false && item.IsActive == true && item.ProductID == productID, includeProperties: "Category,Product");
            return _mapper.Map<ProductSizesViewDto>(result);
        }

        public async Task<IEnumerable<ProductSizesViewDto>> GetProductSizeByCategoryID(int categoryID)
        {
            var productSizes = await GetAllProductSizes();
            var results = productSizes.Where(item => item.Product.CategoryID == categoryID);
            return _mapper.Map<IEnumerable<ProductSizesViewDto>>(results);
        }

        public async Task<IEnumerable<ProductSizesViewDto>> GetProductName(string productName)
        {
            var results = await _productSizesRepository.GetAllAsync(item => item.IsDeleted == false && item.IsActive == true && item.Product.ProductName.Contains(productName), includeProperties: "Category,Product");
            return _mapper.Map<IEnumerable<ProductSizesViewDto>>(results);
        }

        public async Task<bool> UpdateProductSize(ProductSizeDto productSizeDto)
        {
            ArgumentNullException.ThrowIfNull(nameof(productSizeDto));
            ArgumentNullException.ThrowIfNull(nameof(productSizeDto.ProductID));
            ArgumentNullException.ThrowIfNull(nameof(productSizeDto.SizeID));
            // Check Value for Price và OriginalPrice
            if (productSizeDto.Price < 0)
            {
                throw new ArgumentException("Price cannot be negative.", nameof(productSizeDto.Price));
            }

            if (productSizeDto.OriginalPrice < 0)
            {
                throw new ArgumentException("OriginalPrice cannot be negative.", nameof(productSizeDto.OriginalPrice));
            }
            try
            {
                var productExist = await _productRepository.GetAsync(p => p.ProductID == productSizeDto.ProductID);
                var sizeExist = await _sizeRepository.GetAsync(s => s.SizeID == productSizeDto.SizeID);

                if (productExist != null && sizeExist != null)
                {
                    return false;
                }
                else
                {
                    ProductSize product = _mapper.Map<ProductSize>(productSizeDto);
                    await _productSizesRepository.UpdateAsync(product);
                    return true;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
