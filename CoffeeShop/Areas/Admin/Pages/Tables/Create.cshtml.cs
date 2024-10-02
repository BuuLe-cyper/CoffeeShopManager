using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using DataAccess.DataContext;
using DataAccess.Models;
using BussinessObjects.Services;
using CoffeeShop.ViewModels.Tables;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;

namespace CoffeeShop.Areas.Admin.Pages.Tables
{
    [Authorize(Roles = "Admin")]
    public class CreateModel : PageModel
    {
        private readonly ITableService _tableService;
        private readonly IMapper _mapper;

        public CreateModel(ITableService tableService, IMapper mapper)
        {
            _tableService = tableService;
            _mapper = mapper;
        }

        [BindProperty]
        public string Description { get; set; }

        public TableVM Table { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            try
            {
                if (string.IsNullOrWhiteSpace(Description))
                {
                    return Page();
                }

                Table = _mapper.Map<TableVM>(await _tableService.CreateTableAsync(Description));
                return Page();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
           
        }
    }
}
