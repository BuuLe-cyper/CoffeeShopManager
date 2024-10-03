using AutoMapper;
using BussinessObjects.DTOs;
using BussinessObjects.Services;
using CoffeeShop.Helper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CoffeeShop.Areas.Admin.Pages.Category
{
    [Authorize(Roles = "Admin")]
    public class CreateCategoryModel : PageModel
    {
        private readonly ICategoryService _categoryService;
        private readonly IMapper _mapper;

        public CreateCategoryModel(ICategoryService categoryService, IMapper mapper)
        {
            _categoryService = categoryService;
            _mapper = mapper;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public DataAccess.Models.Category Category { get; set; } = default!;

        // For more information, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            ArgumentNullException.ThrowIfNull(nameof(Category.CategoryName));
            Category.CategoryName = Category.CategoryName.Trim();
            bool isValidData = Validations.IsString(Category.CategoryName);
            if (isValidData)
            {
                CategoryDto cateDto = _mapper.Map<CategoryDto>(Category);
                var isAdd = await _categoryService.AddCategory(cateDto);
                if (!isAdd)
                {
                    ModelState.AddModelError(string.Empty, "Unable to create size , category name existed. Please try again.");
                    return Page();
                }
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Size Must Be String. Please try again.");
                return Page();
            }
            return RedirectToAction("ListCategories", "Category", new { area = "Admin" });
        }
    }
}
