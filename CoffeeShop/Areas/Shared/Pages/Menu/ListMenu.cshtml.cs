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
        private readonly IMapper _mapper;
        public ListMenuModel(IProductSizesService productSizesService, IMapper mapper)
        {
            _productSizesService = productSizesService;
            _mapper = mapper;
        }
        public PaginatedList<ProductSizeVM> ProductSize { get; set; }
        public string SearchQuery { get; set; }
        public string CurrentSort { get; set; }
        public int PageIndex { get; set; } = 1;

        public async Task OnGetAsync(string searchQuery, int pageIndex = 1, int pageSize = 3)
        {
            SearchQuery = searchQuery;
            var result = await _productSizesService.GetAllProductSizes();

            var filteredResults = result
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
        }
    }
}