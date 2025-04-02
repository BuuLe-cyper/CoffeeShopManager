using AutoMapper;
using BussinessObjects.DTOs;
using BussinessObjects.Services;
using CoffeeShop.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Net.payOS.Types;
using Net.payOS;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Authorization;
using System.Text;

namespace CoffeeShop.Areas.Shared.Pages.Order
{
    [AllowAnonymous]
    public class CartModel : PageModel
    {
		private readonly IHttpClientFactory _httpClientFactory;
		private readonly string _orderApiUrl = "https://localhost:7158/api/Order"; 
		private readonly string _orderDetailApiUrl = "https://localhost:7158/api/OrderDetail";
		private readonly PayOS _payOS;

		public CartModel(IHttpClientFactory httpClientFactory, PayOS payOS)
		{
			_httpClientFactory = httpClientFactory;
			_payOS = payOS;
		}

		public OrderVM Order { get; set; }
		public IEnumerable<OrderDetailVM> OrderDetails { get; set; } = default!;
		public string TableId { get; set; }

		public async Task OnGetAsync(string? tableId)
        {
            TableId = tableId;
        }

		public async Task<IActionResult> OnPostCheckoutAsync(string cartData, CreatePaymentLinkRequest body)
		{
			if (string.IsNullOrEmpty(cartData))
			{
				return BadRequest("Cart data is empty or invalid.");
			}

			try
			{
				var cart = JsonConvert.DeserializeObject<CartData>(cartData);
				if (cart == null)
				{
					return BadRequest("Unable to deserialize the cart data.");
				}

				// Chưa tạo Order ngay, chỉ tạo Payment nếu BankTransfer
				if (!string.IsNullOrEmpty(cart.paymentMethod) && cart.paymentMethod.Equals("BankTransfer"))
				{
					body.UserInfor = cart.UserId.ToString();
					body.TotalPrice = cart.TotalAmount;

					try
					{
						int orderCode = int.Parse(DateTimeOffset.Now.ToString("ffffff"));
						ItemData item = new ItemData(body.UserInfor, 1, (int)Math.Round(cart.TotalAmount));
						List<ItemData> items = new List<ItemData> { item };

						cartData = JsonConvert.SerializeObject(cart);

						string returnSuccessUrlWithTableId = $"{body.returnUrl}?cartData={Uri.EscapeDataString(cartData)}&tableId={cart.TableId}";
						string returnFailUrlWithTableId = $"{body.cancelUrl}?tableId={cart.TableId}";

						PaymentData paymentData = new PaymentData(orderCode, (int)body.TotalPrice, body.Description, items, returnFailUrlWithTableId, returnSuccessUrlWithTableId);

						CreatePaymentResult paymentResult = await _payOS.createPaymentLink(paymentData);

						return Redirect(paymentResult.checkoutUrl);

						//if (paymentResult.status?.ToUpper() == "SUCCESS")
						//{
						//	HttpContext.Session.SetString("PendingCart", JsonConvert.SerializeObject(cart));
						//	return Redirect(paymentResult.checkoutUrl ?? "/");
						//}
					}
					catch (Exception ex)
					{
						return Content($"Error generating payment link: {ex.Message}");
					}
				}

				// Nếu không dùng BankTransfer, tạo Order ngay
				return await ProcessOrder(cart);
			}
			catch (JsonException ex)
			{
				return BadRequest($"Failed to deserialize cart data: {ex.Message}");
			}
		}

		private async Task<HttpResponseMessage> CreateOrderAsync(OrderVM orderVM)
		{
			using (var client = _httpClientFactory.CreateClient())
			{
				var content = new StringContent(JsonConvert.SerializeObject(orderVM), Encoding.UTF8, "application/json");
				var response = await client.PostAsync(_orderApiUrl, content);
				return response;
			}
		}

		private async Task<HttpResponseMessage> AddOrderDetailAsync(OrderDetailVM orderDetailVM)
		{
			using (var client = _httpClientFactory.CreateClient())
			{
				var content = new StringContent(JsonConvert.SerializeObject(orderDetailVM), Encoding.UTF8, "application/json");
				var response = await client.PostAsync(_orderDetailApiUrl, content); // Use the OrderDetail API URL
				return response;
			}
		}

		private async Task<IActionResult> ProcessOrder(CartData cart)
		{
			Guid userId = cart.UserId.HasValue ? cart.UserId.Value : Guid.NewGuid();

			var orderVM = new OrderVM
			{
				OrderId = Guid.NewGuid(),
				UserID = userId,
				OrderDate = DateTime.Now,
				PaymentMethod = cart.paymentMethod,
				TotalAmount = cart.TotalAmount,
				TableID = cart.TableId,
			};

			var orderResponse = await CreateOrderAsync(orderVM);
			if (!orderResponse.IsSuccessStatusCode)
			{
				return StatusCode(500, "Failed to create order.");
			}

			var orderId = orderVM.OrderId;
			HttpContext.Session.SetString("CurrentOrderId", orderId.ToString());
			HttpContext.Session.SetString("UserId", userId.ToString());

			foreach (var item in cart.CartItems)
			{
				var orderDetail = new OrderDetailVM
				{
					OrderID = orderId,
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

			return Redirect($"/Shared/Order/Bill/{1}");
		}
	}
}
