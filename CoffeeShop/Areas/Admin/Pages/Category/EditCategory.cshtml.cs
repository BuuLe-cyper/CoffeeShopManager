using AutoMapper;
using BussinessObjects.DTOs;
using BussinessObjects.Services;
using CoffeeShop.Helper;
using CoffeeShop.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace CoffeeShop.Areas.Admin.Pages.Category
{
    [Authorize(Roles = "Admin")]
    public class EditCategoryModel : PageModel
    {
        private readonly ICategoryService _categoryService;
        private readonly IMapper _mapper;
        public EditCategoryModel(ICategoryService categoryService, IMapper mapper)
        {
            _categoryService = categoryService;
            _mapper = mapper;
        }

        [BindProperty]
        public CategoryVM Category { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cate = await _categoryService.GetCategory((int)id);
            if (cate == null)
            {
                return NotFound();
            }
            Category = _mapper.Map<CategoryVM>(cate);
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more information, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            ArgumentNullException.ThrowIfNull(nameof(Category.CategoryName));
            Category.CategoryName = Category.CategoryName.ToUpper().Trim();
            bool isValidData = Validations.IsString(Category.CategoryName);
            try
            {
                if (isValidData)
                {
                    var isUpdate = await _categoryService.UpdateCategory(_mapper.Map<CategoryViewDto>(Category));

                    if (!isUpdate)
                    {
                        ModelState.AddModelError(string.Empty, "Unable to Update because Name Exits. Please try again.");
                        return Page();
                    }
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Category Name Must Be String. Please try again.");
                    return Page();
                }
            }
            catch (DbUpdateConcurrencyException)
            {
                throw;
            }

            return RedirectToPage("/Category/ListCategories", new { area = "Shared" });
        }
    }
}
