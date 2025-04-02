using AutoMapper;
using BussinessObjects.Services;
using CoffeeShop.ViewModels;
using DataAccess.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;

namespace CoffeeShop.Areas.Admin.Pages.Statistics
{
    [Authorize(Policy = "AdminOnly")]
    public class ViewOrderDetailModel : PageModel
    {
		private readonly IHttpClientFactory _httpClientFactory;
		private readonly IMapper _mapper;

		public ViewOrderDetailModel(IHttpClientFactory httpClientFactory, IMapper mapper)
		{
			_httpClientFactory = httpClientFactory;
			_mapper = mapper;
		}

		public IEnumerable<OrderDetailVM> OrderDetails { get; set; } = default!;
        public IEnumerable<OrderVM> Orders { get; set; } = default!;

		public async Task<IActionResult> OnGetAsync(Guid id)
		{
			var client = _httpClientFactory.CreateClient();

			IEnumerable<OrderDetailVM> orderDetailVMs = new List<OrderDetailVM>();
			IEnumerable<OrderVM> orderVMs = new List<OrderVM>();

			try
			{
				// L?y thông tin ??n hàng t? API
				var orderResponse = await client.GetAsync($"https://localhost:7158/api/Order/{id}");
				if (orderResponse.IsSuccessStatusCode)
				{
					string orderJson = await orderResponse.Content.ReadAsStringAsync();
					orderVMs = JsonConvert.DeserializeObject<IEnumerable<OrderVM>>(orderJson) ?? new List<OrderVM>();
				}

				// L?y thông tin chi ti?t ??n hàng t? API
				var orderDetailResponse = await client.GetAsync($"https://localhost:7158/api/OrderDetail/{id}");
				if (orderDetailResponse.IsSuccessStatusCode)
				{
					string orderDetailJson = await orderDetailResponse.Content.ReadAsStringAsync();
					orderDetailVMs = JsonConvert.DeserializeObject<IEnumerable<OrderDetailVM>>(orderDetailJson) ?? new List<OrderDetailVM>();
				}
			}
			catch (Exception ex)
			{
				ModelState.AddModelError(string.Empty, "Error calling API: " + ex.Message);
			}

			// Gán giá tr? vào thu?c tính c?a ViewModel
			OrderDetails = orderDetailVMs;
			Orders = orderVMs;

			return Page();
		}
	}
}
