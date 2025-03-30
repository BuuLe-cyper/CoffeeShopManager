using AutoMapper;
using BussinessObjects.DTOs.Message;
using BussinessObjects.Services;
using CoffeeShop.ViewModels.Message;
using Microsoft.AspNetCore.Mvc;

namespace CoffeeShopAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MessagesController : ControllerBase
    {
        private readonly IMessService _messService;
        private readonly IMapper _mapper;

        public MessagesController(IMessService messService, IMapper mapper)
        {
            _messService = messService;
            _mapper = mapper;
        }

        [HttpDelete("CleanOldMessages")]
        public async Task<IActionResult> DeleteOldMessages()
        {
            await _messService.DeleteMessAsyncByHour();
            return NoContent();
        }

        [HttpGet("ByTable/{tableId}")]
        public async Task<ActionResult<IEnumerable<MessageVM>>> GetMessagesByTableId(int tableId)
        {
            var messages = await _messService.GetMessageByTableId(tableId);
            var mapped = _mapper.Map<IEnumerable<MessageVM>>(messages);
            return Ok(mapped);
        }

        [HttpPost]
        public async Task<IActionResult> CreateMessage([FromBody] MessageVM message)
        {
            var dto = _mapper.Map<MessageDTO>(message);
            await _messService.CreateMessage(dto);
            return Ok();
        }
    }
}
