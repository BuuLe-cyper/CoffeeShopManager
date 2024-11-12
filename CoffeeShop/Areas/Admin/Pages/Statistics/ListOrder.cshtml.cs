using AutoMapper;
using BussinessObjects.Services;
using CoffeeShop.Helper;
using CoffeeShop.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;

namespace CoffeeShop.Areas.Admin.Pages.Statistics
{
    [Authorize(Policy = "AdminOnly")]
    public class ListOrderModel : PageModel
    {
        private readonly IOrderService _orderService;
        private readonly IMapper _mapper;

        public ListOrderModel(IOrderService orderService, IMapper mapper)
        {
            _orderService = orderService;
            _mapper = mapper;
        }

        public PaginatedList<OrderVM> Orders { get; set; } = default!;
        public SelectList PageSizeList { get; set; } = new(new[] { 5, 10, 15, 20 }, selectedValue: 5);
        public async Task<IActionResult> OnGetAsync(int pageIndex = 1, int pageSize = 10, string filter = null)
        {
            var allOrders = await _orderService.GetAllOrder();

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
            else if (filter == "all" || filter == null)
            {
               
            }

            var orders = allOrders.OrderByDescending(order => order.OrderDate).ToList();

            List<OrderVM> orderVMs = new();
            foreach (var order in orders)
            {
                var vmOrder = _mapper.Map<OrderVM>(order);
                orderVMs.Add(vmOrder);
            }

            Orders = PaginatedList<OrderVM>.Create(orderVMs, pageIndex, pageSize);

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
