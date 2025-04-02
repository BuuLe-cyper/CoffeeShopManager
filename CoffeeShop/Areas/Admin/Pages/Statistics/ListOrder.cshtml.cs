using AutoMapper;
using CoffeeShop.Helper;
using CoffeeShop.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using System.Globalization;

namespace CoffeeShop.Areas.Admin.Pages.Statistics
{
    [Authorize(Policy = "AdminOnly")]
    public class ListOrderModel : PageModel
    {
		private readonly IHttpClientFactory _httpClientFactory;
		private readonly IMapper _mapper;
		private readonly string _baseUrlApi;

		public ListOrderModel(IHttpClientFactory httpClientFactory, IMapper mapper, IConfiguration configuration)
		{
			_httpClientFactory = httpClientFactory;
			_mapper = mapper;
			_baseUrlApi = configuration["BaseUrlApi"];
		}

		public PaginatedList<OrderVM> Orders { get; set; } = default!;
        public SelectList PageSizeList { get; set; } = new(new[] { 5, 10, 15, 20 }, selectedValue: 5);
		public async Task<IActionResult> OnGetAsync(int pageIndex = 1, int pageSize = 10, string filter = null)
		{
			var client = _httpClientFactory.CreateClient();
			var allOrders = new List<OrderVM>();

			try
			{
				// Gọi API để lấy danh sách tất cả đơn hàng
				var response = await client.GetAsync($"{_baseUrlApi}/api/Order");
				if (response.IsSuccessStatusCode)
				{
					string json = await response.Content.ReadAsStringAsync();
					allOrders = JsonConvert.DeserializeObject<List<OrderVM>>(json) ?? new List<OrderVM>();
				}
			}
			catch (Exception ex)
			{
				ModelState.AddModelError(string.Empty, "Error calling API: " + ex.Message);
			}

			// Áp dụng bộ lọc
			if (filter == "week")
			{
				allOrders = allOrders.Where(order => IsInThisWeek(order.OrderDate)).ToList();
			}
			else if (filter == "month")
			{
				allOrders = allOrders.Where(order => IsInThisMonth(order.OrderDate)).ToList();
			}
			else if (filter == "day")
			{
				allOrders = allOrders.Where(order => order.OrderDate.Date == DateTime.Now.Date).ToList();
			}
			// Nếu filter là "all" hoặc null, không lọc
			else if (filter == "all" || filter == null)
			{
				// Không làm gì, giữ nguyên tất cả đơn hàng
			}

			// Sắp xếp danh sách đơn hàng theo ngày giảm dần
			var orders = allOrders.OrderByDescending(order => order.OrderDate).ToList();

			// Tạo danh sách phân trang
			Orders = PaginatedList<OrderVM>.Create(orders, pageIndex, pageSize);

			return Page();
		}
		private bool IsInThisWeek(DateTime orderDate)
		{
			var currentWeek = CultureInfo.InvariantCulture.Calendar.GetWeekOfYear(DateTime.Now, CalendarWeekRule.FirstDay, DayOfWeek.Monday);
			var orderWeek = CultureInfo.InvariantCulture.Calendar.GetWeekOfYear(orderDate, CalendarWeekRule.FirstDay, DayOfWeek.Monday);
			return currentWeek == orderWeek;
		}

		private bool IsInThisMonth(DateTime orderDate)
		{
			return orderDate.Month == DateTime.Now.Month && orderDate.Year == DateTime.Now.Year;
		}
	}
}
