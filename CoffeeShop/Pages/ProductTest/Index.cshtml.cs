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

namespace CoffeeShop.Pages.ProductTest
{
    public class IndexModel : PageModel
    {
        private readonly IProductService _productService;
        private readonly IMapper _mapper;
        public IndexModel(IProductService productService, IMapper mapper)
        {
            _productService = productService;
            _mapper = mapper;
        }

        public IEnumerable<ProductVM> Product { get; set; } = default!;

        public async Task OnGetAsync()
        {
            var result = await  _productService.GetAllProduct();
            Product = _mapper.Map<IEnumerable<ProductVM>>(result);
        }
    }
}