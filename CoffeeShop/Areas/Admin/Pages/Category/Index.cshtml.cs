using AutoMapper;
using BussinessObjects.Services;
using CoffeeShop.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CoffeeShop.Areas.Admin.Pages.Category
{
    public class IndexModel : PageModel
    {
        private readonly ICategoryService _categoryService;
        private readonly IMapper _mapper;
        public IndexModel(ICategoryService categoryService, IMapper mapper)
        {
            _categoryService = categoryService;
            _mapper = mapper;
        }

        public IEnumerable<CategoryVM> Category { get; set; } = default!;

        public async Task OnGetAsync()
        {
            var result = await _categoryService.GetAllCategory();
            Category = _mapper.Map<IEnumerable<CategoryVM>>(result);
        }
    }
}
