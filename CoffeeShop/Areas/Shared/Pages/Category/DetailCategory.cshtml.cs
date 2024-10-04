using AutoMapper;
using BussinessObjects.Services;
using CoffeeShop.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CoffeeShop.Areas.Shared.Pages.Category
{
    public class DetailCategoryModel : PageModel
    {
        private readonly ICategoryService _cateService;
        private readonly IMapper _mapper;
        public DetailCategoryModel(ICategoryService cateService, IMapper mapper)
        {
            _cateService = cateService;
            _mapper = mapper;
        }

        public CategoryVM Category { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            //var product = await _context.Products.FirstOrDefaultAsync(m => m.ProductID == id);
            var category = await _cateService.GetCategory((int)id);
            if (category == null)
            {
                return NotFound();
            }
            else
            {
                Category = _mapper.Map<CategoryVM>(category);
            }
            return Page();
        }
    }
}