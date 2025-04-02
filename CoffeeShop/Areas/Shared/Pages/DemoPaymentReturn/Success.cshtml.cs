using CoffeeShop.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using System.Text;

namespace CoffeeShop.Areas.Shared.Pages.DemoPaymentReturn
{
	[AllowAnonymous]
	public class SuccessModel : PageModel
	{
		private readonly IHttpClientFactory _httpClientFactory;
		private readonly string _baseUrlApi;

		public SuccessModel(IHttpClientFactory httpClientFactory, IConfiguration configuration)
		{
			_httpClientFactory = httpClientFactory;
			_baseUrlApi = configuration["BaseUrlApi"];
		}

		public async Task<IActionResult> OnGetAsync(string cartData, string tableId)
		{
			if (string.IsNullOrEmpty(cartData))
			{
				return BadRequest("Cart data is missing.");
			}

			try
			{
				var cart = JsonConvert.DeserializeObject<CartData>(cartData);

				if (cart == null)
				{
					return BadRequest("Unable to deserialize the cart data.");
				}

				// Create Order
				var orderVM = new OrderVM
				{
					OrderId = Guid.NewGuid(),
					UserID = cart.UserId ?? Guid.NewGuid(),
					OrderDate = DateTime.Now,
					PaymentMethod = cart.paymentMethod,
					TotalAmount = cart.TotalAmount,
					TableID = int.Parse(tableId),
				};

				// Call API to create order
				var orderResponse = await CreateOrderAsync(orderVM);
				if (!orderResponse.IsSuccessStatusCode)
				{
					return StatusCode(500, "Failed to create order.");
				}

				// Add Order Details
				foreach (var item in cart.CartItems)
				{
					var orderDetail = new OrderDetailVM
					{
						OrderID = orderVM.OrderId,
						ProductSizeID = item.ProductSizeID,
						UnitPrice = item.UnitPrice,
						Quantity = item.Quantity,
						Discount = item.Discount,
						ProductName = item.ProductName,
						SizeName = ""
					};

					var orderDetailResponse = await AddOrderDetailAsync(orderDetail);
					if (!orderDetailResponse.IsSuccessStatusCode)
					{
						return StatusCode(500, "Failed to add order detail.");
					}
				}

				// Redirect to Bill page
				return Redirect($"/Shared/Order/Bill/{orderVM.OrderId}/{orderVM.TableID}");
			}
			catch (JsonException ex)
			{
				return BadRequest($"Failed to deserialize cart data: {ex.Message}");
			}
		}

		private async Task<HttpResponseMessage> CreateOrderAsync(OrderVM orderVM)
		{
			string _orderApiUrl = $"{_baseUrlApi}/api/Order";
			using (var client = _httpClientFactory.CreateClient())
			{
				var content = new StringContent(JsonConvert.SerializeObject(orderVM), Encoding.UTF8, "application/json");
				var response = await client.PostAsync(_orderApiUrl, content);
				return response;
			}
		}

		private async Task<HttpResponseMessage> AddOrderDetailAsync(OrderDetailVM orderDetailVM)
		{
			string _orderDetailApiUrl = $"{_baseUrlApi}/api/OrderDetail";
			using (var client = _httpClientFactory.CreateClient())
			{
				var content = new StringContent(JsonConvert.SerializeObject(orderDetailVM), Encoding.UTF8, "application/json");
				var response = await client.PostAsync(_orderDetailApiUrl, content);
				return response;
			}
		}
	}
}

