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
using AutoMapper;
using CoffeeShop.ViewModels;

namespace CoffeeShop.Pages.SizeTest
{
    public class IndexModel : PageModel
    {
        private readonly ISizeService _sizeService;
        private readonly IMapper _mapper;
        public IndexModel(ISizeService sizeService , IMapper mapper)
        {
            _sizeService = sizeService;
            _mapper = mapper;
        }

        public IEnumerable<SizeVM> Size { get;set; } = default!;

        public async Task OnGetAsync()
        {
            var result =  await _sizeService.GetAllSize();
            Size = _mapper.Map<IEnumerable<SizeVM>>(result);
        }
    }
}
