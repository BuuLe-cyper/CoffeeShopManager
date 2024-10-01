using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using DataAccess.DataContext;
using DataAccess.Models;
using BussinessObjects.Services;
using CoffeeShop.ViewModels;
using AutoMapper;

namespace CoffeeShop.Pages.CategoryTest
{
    public class DeleteModel : PageModel
    {
        private readonly ICategoryService _categoryService;
        private readonly IMapper _mapper;

        public DeleteModel(ICategoryService categoryService, IMapper mapper)
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

            return RedirectToPage("./Index");
        }
    }
}
