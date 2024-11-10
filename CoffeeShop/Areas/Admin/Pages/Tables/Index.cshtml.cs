using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using DataAccess.DataContext;
using DataAccess.Models;
using CoffeeShop.ViewModels.Tables;
using Microsoft.EntityFrameworkCore.Metadata;
using BussinessObjects.Services;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using CoffeeShop.Helper;
using System.Drawing.Printing;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace CoffeeShop.Areas.Admin.Pages.Tables
{
    [Authorize(Roles = "Admin")]
    public class IndexModel : PageModel
    {
        private readonly ITableService _tableService;
        private readonly IMapper _mapper;

        public IndexModel(ITableService tableService, IMapper mapper)
        {
            _tableService = tableService;
            _mapper = mapper;
        }
        public string CurrentFilter { get; set; }
        public PaginatedList<TableVM> Table { get;set; } = default!;
        public async Task OnGetAsync(string currentFilter, string searchString, int? pageIndex)
        {
            var tableVMs = _mapper.Map<IEnumerable<TableVM>>(await _tableService.GetAllAsync());
            var tableIQ = tableVMs.AsQueryable();
            if (!String.IsNullOrEmpty(searchString))
            {
                tableVMs = tableVMs.Where(s => s.Description.Contains(searchString)).AsQueryable();
            }
            var pageSize = 5;
            var count = tableVMs.Count();
            var paginatedList = tableVMs.Skip((pageIndex ?? 1 - 1) * pageSize)
                                        .Take(pageSize)
                                        .ToList();
            Table = new PaginatedList<TableVM>(paginatedList, count, pageIndex ?? 1, pageSize);
        }


        public async Task<IActionResult> OnGetDownloadQRCode(int id)
        {
            var table = _mapper.Map<TableVM>(await _tableService.GetAsync(id));
            if (table == null)
            {
                return NotFound();
            }

            var base64Data = table.QRCodeTable.Split(',')[1];
            var imageBytes = Convert.FromBase64String(base64Data);

            return File(imageBytes, "image/png", $"QRCode_{id}.png");
        }

    }
}
