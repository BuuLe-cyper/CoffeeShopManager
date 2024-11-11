using AutoMapper;
using BussinessObjects.Services;
using CoffeeShop.ViewModels;
using DataAccess.Models;
using DataAccess.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CoffeeShop.Areas.Shared.Pages.Order
{
    public class BillModel : PageModel
    {
        private readonly IOrderDetailService _orderDetailService;
        private readonly IOrderService _orderService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMapper _mapper;
        public BillModel(IOrderDetailService orderDetailService, IMapper mapper, IOrderService orderService, IHttpContextAccessor httpContextAccessor)
        {
            _orderDetailService = orderDetailService;
            _orderService = orderService;
            _httpContextAccessor = httpContextAccessor;
            _mapper = mapper;
        }

        public IEnumerable<OrderDetailVM> OrderDetails { get; set; } = default!;
        public IEnumerable<OrderVM> Orders { get; set; } = default!;

        public async Task OnGetAsync(Guid orderId)
        {
            var currentOrderId = _httpContextAccessor.HttpContext.Session.GetString("CurrentOrderId");
            orderId = new Guid(currentOrderId);

            //Guid orderID = new Guid("8c3adbad-2410-4ca6-8211-cd7c8190c5c0");

            var orderDetail = await _orderDetailService.GetOrderDetailsByOrderId(orderId);
            var order = await _orderService.GetOrderByOrderId(orderId);

            OrderDetails = orderDetail != null ? _mapper.Map<IEnumerable<OrderDetailVM>>(orderDetail) : new List<OrderDetailVM>();
            Orders = order != null ? _mapper.Map<IEnumerable<OrderVM>>(order) : new List<OrderVM>();

        }
    }
}
