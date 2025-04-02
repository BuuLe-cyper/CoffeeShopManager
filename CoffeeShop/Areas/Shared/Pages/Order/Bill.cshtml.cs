using BussinessObjects.Services;
using CoffeeShop.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;

namespace CoffeeShop.Areas.Shared.Pages.Order
{
    [AllowAnonymous]
    public class BillModel : PageModel
    {
		private readonly IHttpClientFactory _httpClientFactory;
		private readonly IHttpContextAccessor _httpContextAccessor;
		private readonly IMessService _messService;

		public BillModel(IHttpClientFactory httpClientFactory, IHttpContextAccessor httpContextAccessor, IMessService messService)
		{
			_httpClientFactory = httpClientFactory;
			_httpContextAccessor = httpContextAccessor;
			_messService = messService;
		}

		public IEnumerable<OrderDetailVM> OrderDetails { get; set; } = default!;
        public OrderVM Order { get; set; } = default!;
        public int TableId { get; set; }

		public async Task OnGetAsync(Guid orderId, int tableId)
		{
			// Cập nhật tin nhắn theo TableId
			await _messService.UpdateMessagesByTableIdAsync(tableId);

			// Nếu orderId trong session không hợp lệ, sử dụng orderId từ route
			var currentOrderId = _httpContextAccessor.HttpContext.Session.GetString("CurrentOrderId");
			if (Guid.TryParse(currentOrderId, out Guid sessionOrderId) && sessionOrderId != Guid.Empty)
			{
				orderId = sessionOrderId;
			}

			var client = _httpClientFactory.CreateClient();

			try
			{
				// Lấy Order Details từ API
				string orderDetailUrl = $"https://localhost:7158/api/OrderDetail/{orderId}";
				HttpResponseMessage orderDetailResponse = await client.GetAsync(orderDetailUrl);

				if (orderDetailResponse.IsSuccessStatusCode)
				{
					string jsonOrderDetails = await orderDetailResponse.Content.ReadAsStringAsync();
					OrderDetails = JsonConvert.DeserializeObject<IEnumerable<OrderDetailVM>>(jsonOrderDetails) ?? new List<OrderDetailVM>();
				}
				else
				{
					OrderDetails = new List<OrderDetailVM>();
				}

				// Lấy Order từ API
				string orderUrl = $"https://localhost:7158/api/Order/{orderId}";
				HttpResponseMessage orderResponse = await client.GetAsync(orderUrl);

				if (orderResponse.IsSuccessStatusCode)
				{
					string jsonOrder = await orderResponse.Content.ReadAsStringAsync();
					// Deserialize thành List<OrderVM>, nhưng chỉ lấy phần tử đầu tiên nếu có
					var ordersList = JsonConvert.DeserializeObject<List<OrderVM>>(jsonOrder);
					Order = ordersList?.FirstOrDefault() ?? new OrderVM();
				}
				else
				{
					Order = new OrderVM();
				}

				TableId = tableId; // Gán giá trị TableId từ route
			}
			catch (Exception ex)
			{
				ModelState.AddModelError(string.Empty, "Error calling API: " + ex.Message);
				Order = new OrderVM();
				OrderDetails = new List<OrderDetailVM>();
			}
		}
	}
}
