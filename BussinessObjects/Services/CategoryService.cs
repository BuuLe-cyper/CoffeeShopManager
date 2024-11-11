using AutoMapper;
using BussinessObjects.DTOs;
using DataAccess.Models;
using DataAccess.Repositories;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BussinessObjects.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IProductRepository _productRepository;
        private readonly IMapper _mapper;
        public CategoryService(ICategoryRepository categoryRepository,IProductRepository productRepository,IMapper mapper)
        {
            _categoryRepository = categoryRepository;
            _productRepository = productRepository;
            _mapper = mapper;
        }
        public async Task<bool> AddCategory(CategoryDto? categoryDto)
        {
            ArgumentNullException.ThrowIfNull(nameof(categoryDto.CategoryName));
            try
            {
                // if category be softdelete it will be create again with old category have IsDeleted == true and IsActive == false
                categoryDto.CategoryName = categoryDto.CategoryName.Trim().ToUpper();
                var cateNameExist = await GetCategoryName(categoryDto.CategoryName);
                var deletedCategoryName = await _categoryRepository.GetAsync(item => item.CategoryName == categoryDto.CategoryName
                                                                          && item.IsDeleted == true && item.IsActive == false);
                if (cateNameExist == null && (deletedCategoryName != null || deletedCategoryName == null))
                {
                    var entity = _mapper.Map<Category>(categoryDto);
                    await _categoryRepository.CreateAsync(entity);
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

        public async Task<IEnumerable<CategoryViewDto>> GetAllCategory()
        {
            var categories = await _categoryRepository.GetAllAsync(c => c.IsDeleted == false && c.IsActive == true);
            return _mapper.Map<IEnumerable<CategoryViewDto>>(categories);
        }

        public async Task<CategoryViewDto> GetCategory(int categoryID)
        {
            var category = await _categoryRepository.GetAsync(c => c.IsDeleted == false && c.IsActive == true && c.CategoryID == categoryID);
            return _mapper.Map<CategoryViewDto>(category);
        }

        public async Task<CategoryViewDto> GetCategoryName(string categoryName)
        {
            var cate = await _categoryRepository.GetAsync(c => c.IsDeleted == false && c.IsActive == true && c.CategoryName == categoryName);
            return _mapper.Map<CategoryViewDto>(cate);
        }

        public async Task<bool> SoftDeleteCategory(int categoryID)
        {
            // If the Category is deleted, equivalent products will automatically be unavailable
            try
            {
                var categoryWithProduct = await GetCategory(categoryID);
                List<Product> products = categoryWithProduct.Products.ToList();
                if (products.Any())
                {
                    foreach (var product in products)
                    {
                        product.IsAvailable = false;
                        await _productRepository.UpdateAsync(product);
                    }
                }
                return await _categoryRepository.SoftDeleteCategoryEntity(categoryID);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<bool> UpdateCategory(CategoryViewDto? category)
        {
            ArgumentNullException.ThrowIfNull(category, nameof(category));

            try
            {
                var sizeExists = await _categoryRepository.GetAsync(c => c.CategoryID == category.CategoryID && c.IsDeleted == false && c.IsActive == true)
                    ?? throw new Exception("Category Not Found");
                var cateNameExist = await GetCategoryName(category.CategoryName);
                if (cateNameExist == null)
                {
                    // Ensuring no permission to modify this field
                    category.IsDeleted = false;
                    category.IsActive = true;
                    category.CreateDate = sizeExists.CreateDate;
                    category.ModifyDate = DateTime.Now;

                    await _categoryRepository.UpdateAsync(_mapper.Map<Category>(category));
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
