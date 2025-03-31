using AutoMapper;
using BussinessObjects.DTOs;
using BussinessObjects.Services;
using CoffeeShop.ViewModels;
using CoffeeShop.ViewModels.Tables;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CoffeeShopAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SizeController : ControllerBase
    {
        private readonly ISizeService _sizeService;
        private readonly IMapper _mapper;

        public SizeController(ISizeService sizeService, IMapper mapper)
        {
            _sizeService = sizeService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllSizes()
        {
            var sizes = await _sizeService.GetAllSize();
            var sizeVMs = _mapper.Map<IEnumerable<SizeVM>>(sizes);
            return Ok(sizeVMs);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetSize(int id)
        {
            var size = await _sizeService.GetSize(id);
            if (size == null)
                return NotFound("Size not found.");

            var sizeVM = _mapper.Map<SizeVM>(size);
            return Ok(sizeVM);
        }


        [HttpPost]
        public async Task<IActionResult> CreateSize([FromBody] SizeDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.sizeName))
            {
                return BadRequest("Size Name cannot be empty.");
            }

            bool isCreated = await _sizeService.AddSize(dto);
            if (!isCreated)
            {
                return BadRequest("Size already exists.");
            }

            return Ok("Size created successfully.");
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateSize(int id, [FromBody] SizeViewDto dto)
        {
            if (id != dto.SizeID)
            {
                return BadRequest("ID mismatch.");
            }

            bool isUpdated = await _sizeService.UpdateSize(dto);
            if (!isUpdated)
            {
                return BadRequest("Unable to update. Size name might already exist.");
            }

            return Ok("Size updated successfully.");
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteSize(int id)
        {
            bool isDeleted = await _sizeService.SoftDeleteSize(id);
            if (!isDeleted)
            {
                return BadRequest("Unable to delete size.");
            }

            return Ok("Size deleted successfully.");
        }
    }
}
