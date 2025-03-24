using AutoMapper;
using BussinessObjects.DTOs;
using BussinessObjects.Services;
using CoffeeShop.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;

namespace CoffeeShopAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _categoryService;
        private readonly IMapper _mapper;

        public CategoryController(ICategoryService categoryService, IMapper mapper)
        {
            _categoryService = categoryService;
            _mapper = mapper;
        }

        [HttpGet]
        [EnableQuery]
        public async Task<IActionResult> GetAllCategories()
        {
            var categories = await _categoryService.GetAllCategory();
            if (categories == null || !categories.Any())
            {
                return NotFound("No categories found.");
            }

            var categoryVMs = _mapper.Map<IEnumerable<CategoryVM>>(categories);
            return Ok(categoryVMs);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetCategoryById(int id)
        {
            var category = await _categoryService.GetCategory(id);
            if (category == null)
            {
                return NotFound($"Category with ID {id} not found.");
            }

            var categoryVM = _mapper.Map<CategoryVM>(category);
            return Ok(categoryVM);
        }

        [HttpPost]
        public async Task<IActionResult> CreateCategory([FromBody] CategoryVM categoryVM)
        {
            if (categoryVM == null || string.IsNullOrWhiteSpace(categoryVM.CategoryName))
            {
                return BadRequest("Category name is required.");
            }

            categoryVM.CategoryName = categoryVM.CategoryName.Trim();

            if (!CoffeeShop.Helper.Validations.IsString(categoryVM.CategoryName))
            {
                return BadRequest("Category name must contain only letters.");
            }

            var categoryDto = _mapper.Map<CategoryDto>(categoryVM);
            bool isCreated = await _categoryService.AddCategory(categoryDto);

            if (!isCreated)
            {
                return Conflict("Category name already exists.");
            }

            return CreatedAtAction(nameof(GetCategoryById), new { id = categoryVM.CategoryID }, categoryDto);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            var category = await _categoryService.GetCategory(id);
            if (category == null)
            {
                return NotFound($"Category with ID {id} not found.");
            }

            bool isDeleted = await _categoryService.SoftDeleteCategory(id);
            if (!isDeleted)
            {
                return StatusCode(500, "Unable to delete category. Please try again.");
            }

            return NoContent(); 
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCategory(int id, [FromBody] CategoryDto categoryDto)
        {
            //if (id != categoryDto.CategoryID)
            //{
            //    return BadRequest("Category ID mismatch.");
            //}

            var existingCategory = await _categoryService.GetCategory(id);
            if (existingCategory == null)
            {
                return NotFound($"Category with ID {id} not found.");
            }

            categoryDto.CategoryName = categoryDto.CategoryName.ToUpper().Trim();
            bool isUpdated = await _categoryService.UpdateCategory(_mapper.Map<CategoryViewDto>(categoryDto));

            if (!isUpdated)
            {
                return Conflict("Unable to update category. Category name might already exist.");
            }

            return NoContent();
        }
    }
}
