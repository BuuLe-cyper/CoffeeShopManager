using AutoMapper;
using BussinessObjects.Services;
using CoffeeShop.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CoffeeShop.Areas.Admin.Pages.Category
{
    [Authorize(Roles = "Admin")]
    public class DeleteCategoryModel : PageModel
    {
        private readonly ICategoryService _categoryService;
        private readonly IMapper _mapper;

        public DeleteCategoryModel(ICategoryService categoryService, IMapper mapper)
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
            else
            {
                Category = _mapper.Map<CategoryVM>(cate);
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cate = await _categoryService.GetCategory((int)id);
            if (cate != null)
            {
                Category = _mapper.Map<CategoryVM>(cate);
                var isRemove = await _categoryService.SoftDeleteCategory(cate.CategoryID);
                if (!isRemove)
                {
                    ModelState.AddModelError(string.Empty, "Unable to delete category. Please try again.");
                    return Page();
                }
            }

            return RedirectToAction("ListCategories", "Category", new { area = "Admin" });
        }
    }
}
