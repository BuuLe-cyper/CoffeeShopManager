using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DataAccess.DataContext;
using DataAccess.Models;
using CoffeeShop.Helper;
using BussinessObjects.Services;
using CoffeeShop.ViewModels;
using AutoMapper;
using BussinessObjects.DTOs;

namespace CoffeeShop.Pages.CategoryTest
{
    public class EditModel : PageModel
    {
        private readonly ICategoryService _categoryService;
        private readonly IMapper _mapper;
        public EditModel(ICategoryService categoryService, IMapper mapper)
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

            return RedirectToPage("./Index");
        }
    }
}
