using AutoMapper;
using CoffeeShop.ViewModels;
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
		private readonly string _baseUrlApi;

		public ViewOrderDetailModel(IHttpClientFactory httpClientFactory, IMapper mapper, IConfiguration configuration)
		{
			_httpClientFactory = httpClientFactory;
			_mapper = mapper;
			_baseUrlApi = configuration["BaseUrlApi"];
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
				var orderResponse = await client.GetAsync($"{_baseUrlApi}/api/Order/{id}");
				if (orderResponse.IsSuccessStatusCode)
				{
					string orderJson = await orderResponse.Content.ReadAsStringAsync();
					orderVMs = JsonConvert.DeserializeObject<IEnumerable<OrderVM>>(orderJson) ?? new List<OrderVM>();
				}

				var orderDetailResponse = await client.GetAsync($"{_baseUrlApi}/api/OrderDetail/{id}");
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

			OrderDetails = orderDetailVMs;
			Orders = orderVMs;

			return Page();
		}
	}
}
