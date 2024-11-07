using AutoMapper;
using BussinessObjects.Services;
using CoffeeShop.ViewModels.Tables;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CoffeeShop.Areas.Admin.Pages.Chats
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
        public List<TableVM> Tables { get; set; }
        public async Task OnGetAsync()
        {
            var tableEntities = await _tableService.GetAllAsync();
            Tables = _mapper.Map<List<TableVM>>(tableEntities);
        }
    }
}
