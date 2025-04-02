using AutoMapper;
using BussinessObjects.DTOs;
using BussinessObjects.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;

namespace CoffeeShopAPI.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class OrderController : ControllerBase
	{
		private readonly IOrderService _orderService;
		private readonly IMapper _mapper;

		public OrderController(IOrderService orderService, IMapper mapper)
		{
			_orderService = orderService;
			_mapper = mapper;
		}

		[HttpPost]
		public async Task<IActionResult> CreateOrder([FromBody] OrderDTO orderDTO)
		{
			if (orderDTO == null)
			{
				return BadRequest("Order data is required.");
			}

			await _orderService.CreateOrder(orderDTO);
			return Ok("Order created successfully.");
		}

		[HttpGet("{orderId}")]
		public async Task<IActionResult> GetOrderByOrderId(Guid orderId)
		{
			var orders = await _orderService.GetOrderByOrderId(orderId);
			if (orders == null || !orders.Any())
			{
				return NotFound("Order not found.");
			}
			return Ok(orders);
		}

		[HttpGet("{orderId}/total-amount")]
		public async Task<IActionResult> CalculateTotalAmount(Guid orderId)
		{
			var totalAmount = await _orderService.CalculateTotalAmount(orderId);
			return Ok(totalAmount);
		}

		[HttpGet("customer/{customerId}")]
		public async Task<IActionResult> GetListOrdersByCustomerId(Guid customerId)
		{
			var orders = await _orderService.GetListOrdersByCustomerId(customerId);
			if (orders == null || !orders.Any())
			{
				return NotFound("No orders found for this customer.");
			}
			return Ok(orders);
		}

		[HttpGet("{orderId}/status")]
		public async Task<IActionResult> GetOrderStatus(Guid orderId)
		{
			var status = await _orderService.GetOrderStatus(orderId);
			if (status == null)
			{
				return NotFound("Order not found.");
			}
			return Ok(status);
		}

		[HttpGet]
		[EnableQuery]
		public async Task<IActionResult> GetAllOrders()
		{
			var orders = await _orderService.GetAllOrder();
			if (orders == null || !orders.Any())
			{
				return NotFound("No orders found.");
			}
			return Ok(orders);
		}
	}
}