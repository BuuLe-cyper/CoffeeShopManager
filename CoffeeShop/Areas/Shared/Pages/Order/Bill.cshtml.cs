using AutoMapper;
using BussinessObjects.Services;
using CoffeeShop.ViewModels;
using DataAccess.Models;
using DataAccess.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CoffeeShop.Areas.Shared.Pages.Order
{
    [AllowAnonymous]
    public class BillModel : PageModel
    {
        private readonly IOrderDetailService _orderDetailService;
        private readonly IOrderService _orderService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMapper _mapper;
        private readonly IMessService _messService;
        public BillModel(IOrderDetailService orderDetailService, IMapper mapper, IOrderService orderService, IHttpContextAccessor httpContextAccessor, IMessService messService)
        {
            _orderDetailService = orderDetailService;
            _orderService = orderService;
            _httpContextAccessor = httpContextAccessor;
            _mapper = mapper;
            _messService = messService;
        }

        public IEnumerable<OrderDetailVM> OrderDetails { get; set; } = default!;
        public IEnumerable<OrderVM> Orders { get; set; } = default!;
        //public int TableId { get; set; }

        public async Task OnGetAsync(Guid orderId, int tableId)
        {
            await _messService.UpdateMessagesByTableIdAsync(tableId);
            var currentOrderId = _httpContextAccessor.HttpContext.Session.GetString("CurrentOrderId");
            orderId = new Guid(currentOrderId);
            var orderDetail = await _orderDetailService.GetOrderDetailsByOrderId(orderId);
            var order = await _orderService.GetOrderByOrderId(orderId);

            OrderDetails = orderDetail != null ? _mapper.Map<IEnumerable<OrderDetailVM>>(orderDetail) : new List<OrderDetailVM>();
            Orders = order != null ? _mapper.Map<IEnumerable<OrderVM>>(order) : new List<OrderVM>();

        }
    }
}
