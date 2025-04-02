using AutoMapper;
using BussinessObjects.DTOs;
using BussinessObjects.Services;
using Microsoft.AspNetCore.Mvc;

namespace CoffeeShopAPI.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class OrderDetailController : ControllerBase
	{
		private readonly IOrderDetailService _orderDetailService;
		private readonly IMapper _mapper;

		public OrderDetailController(IOrderDetailService orderDetailService, IMapper mapper)
		{
			_orderDetailService = orderDetailService;
			_mapper = mapper;
		}

		[HttpGet("{orderId}")]
		public async Task<IActionResult> GetOrderDetailsByOrderId(Guid orderId)
		{
			var orderDetails = await _orderDetailService.GetOrderDetailsByOrderId(orderId);
			if (orderDetails == null || !orderDetails.Any())
			{
				return NotFound("No order details found.");
			}
			return Ok(orderDetails);
		}

		[HttpPost]
		public async Task<IActionResult> AddOrderDetail([FromBody] OrderDetailDTO orderDetailDTO)
		{
			if (orderDetailDTO == null)
			{
				return BadRequest("Order detail data is required.");
			}

			var result = await _orderDetailService.AddOrderDetail(orderDetailDTO);
			if (!result)
			{
				return StatusCode(500, "Failed to add order detail.");
			}

			return Ok("Order detail added successfully.");
		}

		[HttpPut]
		public async Task<IActionResult> UpdateOrderDetail([FromBody] OrderDetailDTO orderDetailDTO)
		{
			if (orderDetailDTO == null)
			{
				return BadRequest("Order detail data is required.");
			}

			var result = await _orderDetailService.UpdateOrderDetail(orderDetailDTO);
			if (!result)
			{
				return StatusCode(500, "Failed to update order detail.");
			}

			return Ok("Order detail updated successfully.");
		}
	}
}
