using AutoMapper;
using BussinessObjects.Services;
using CoffeeShop.Helper;
using CoffeeShop.ViewModels;
using DataAccess.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SkiaSharp;
using System.Drawing.Printing;

namespace CoffeeShop.Areas.Shared.Pages.Menu
{
    public class ListMenuModel : PageModel
    {
        private readonly IProductSizesService _productSizesService;
        private readonly ICategoryService _categoryService;
        private readonly IMapper _mapper;
        public ListMenuModel(IProductSizesService productSizesService, IMapper mapper, ICategoryService categoryService)
        {
            _productSizesService = productSizesService;
            _mapper = mapper;
            _categoryService = categoryService;
        }
        public PaginatedList<ProductSizeVM> ProductSize { get; set; }
        public string SearchQuery { get; set; }
        public string CurrentSort { get; set; }
        public int PageIndex { get; set; } = 1;
        public IEnumerable<CategoryVM> Category { get; set; } = default!;
        public async Task OnGetAsync(string searchQuery, int pageIndex = 1, int pageSize = 3, int cateId = 0)
        {
            SearchQuery = searchQuery;
            var result = await _productSizesService.GetAllProductSizes();
            var resultCate = cateId == 0
                              ? result
                              : result.Where(it => it.Product.CategoryID == cateId);

            var filteredResults = resultCate
                .Where(x => !x.Size.IsDeleted &&
                            (string.IsNullOrEmpty(searchQuery) ||
                             x.Product.ProductName.Contains(searchQuery, StringComparison.OrdinalIgnoreCase) ||
                             x.Size.SizeName.Contains(searchQuery, StringComparison.OrdinalIgnoreCase)))
                .AsQueryable();

            // Group by ProductID
            var groupedResults = filteredResults
                .GroupBy(x => x.ProductID)
                .ToList();

            var count = groupedResults.Count;
            var pageGroups = groupedResults
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            var items = pageGroups.SelectMany(g => g.Select(x => _mapper.Map<ProductSizeVM>(x))).ToList();

            ProductSize = new PaginatedList<ProductSizeVM>(items, count, pageIndex, pageSize);
            var results = await _categoryService.GetAllCategory();
            Category = _mapper.Map<IEnumerable<CategoryVM>>(results);
        }
    }
}