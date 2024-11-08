using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using DataAccess.DataContext;
using DataAccess.Models;
using Microsoft.EntityFrameworkCore.Metadata;
using BussinessObjects.Services;
using CoffeeShop.ViewModels.Tables;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;

namespace CoffeeShop.Areas.Admin.Pages.Tables
{
    [Authorize(Roles = "Admin")]
    public class DetailsModel : PageModel
    {
        private readonly ITableService _tableService;
        private readonly IMapper _mapper;

        public DetailsModel(ITableService tableService, IMapper mapper)
        {
            _tableService = tableService;
            _mapper = mapper;
        }

        public TableVM Table { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var table = _mapper.Map<TableVM>(await _tableService.GetAsync(id));

            if (table == null)
            {
                return NotFound();
            }
            else
            {
                Table = table;
            }
            return Page();
        }
    }
}
