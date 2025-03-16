using AutoMapper;
using BussinessObjects.DTOs.Tables;
using BussinessObjects.Services;
using CoffeeShop.ViewModels.Tables;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;

namespace CoffeeShopAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TablesController : ControllerBase
    {
        private readonly ITableService _tableService;
        private readonly IMapper _mapper;

        public TablesController(ITableService tableService, IMapper mapper)
        {
            _tableService = tableService;
            _mapper = mapper;
        }

        [HttpGet]
        [EnableQuery]
        public async Task<ActionResult<IEnumerable<TableVM>>> GetTables()
        {
            var tables = await _tableService.GetAllAsync();
            var tableVMs = _mapper.Map<IEnumerable<TableVM>>(tables);
            return Ok(tableVMs.AsQueryable());
        }

        [HttpGet("{id}/QRCode")]
        public async Task<IActionResult> GetQRCode(int id)
        {
            var table = await _tableService.GetAsync(id);
            if (table == null)
            {
                return NotFound();
            }

            var tableVM = _mapper.Map<TableVM>(table);
            var base64Data = tableVM.QRCodeTable.Split(',')[1];
            var imageBytes = System.Convert.FromBase64String(base64Data);

            return File(imageBytes, "image/png", $"QRCode_{id}.png");
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetTable(int id)
        {
            var table = await _tableService.GetAsync(id);
            if (table == null)
            {
                return NotFound();
            }

            var tableVM = _mapper.Map<TableVM>(table);
            return Ok(tableVM);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTable(int id)
        {
            var table = await _tableService.GetAsync(id);
            if (table == null)
            {
                return NotFound();
            }

            await _tableService.SoftDeleteTable(id);
            return NoContent();
        }

        [HttpPost]
        public async Task<IActionResult> CreateTable([FromBody] string description)
        {
            if (string.IsNullOrWhiteSpace(description))
            {
                return BadRequest("Description cannot be empty.");
            }

            var table = await _tableService.CreateTableAsync(description);
            var tableVM = _mapper.Map<TableVM>(table);

            return Ok(tableVM);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateTable([FromBody] TableDTO tableDto)
        {
            await _tableService.UpdateTableAsync(tableDto);

            return NoContent();
        }
    }
}
