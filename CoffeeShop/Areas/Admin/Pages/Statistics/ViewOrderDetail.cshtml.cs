using AutoMapper;
using BussinessObjects.Services;
using CoffeeShop.ViewModels;
using DataAccess.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CoffeeShop.Areas.Admin.Pages.Statistics
{
    [Authorize(Policy = "AdminOnly")]
    public class ViewOrderDetailModel : PageModel
    {
        private readonly IOrderDetailService _orderDetailService;
        private readonly IOrderService _orderService;
        private readonly IMapper _mapper;
        public ViewOrderDetailModel(IOrderDetailService orderDetailService, IMapper mapper, IOrderService orderService)
        {
            _orderDetailService = orderDetailService;
            _orderService = orderService;
            _mapper = mapper;
        }
        public IEnumerable<OrderDetailVM> OrderDetails { get; set; } = default!;
        public IEnumerable<OrderVM> Orders { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(Guid id)
        {
            var order = await _orderService.GetOrderByOrderId(id);
            var orderDetail = await _orderDetailService.GetOrderDetailsByOrderId(id);

            if (orderDetail == null)
            {
                return NotFound();
            }

            OrderDetails = orderDetail != null ? _mapper.Map<IEnumerable<OrderDetailVM>>(orderDetail) : new List<OrderDetailVM>();
            Orders = order != null ? _mapper.Map<IEnumerable<OrderVM>>(order) : new List<OrderVM>();

            return Page();

        }

    }
}
