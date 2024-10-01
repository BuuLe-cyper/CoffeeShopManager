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

        public IList<TableVM> Table { get;set; } = default!;

        public async Task OnGetAsync()
        {
            Table = _mapper.Map<IList<TableVM>>(await _tableService.GetAllAsync());
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
