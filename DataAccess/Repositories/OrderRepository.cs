using DataAccess.DataContext;
using DataAccess.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repositories
{
    public class OrderRepository : Repository<Order>, IOrderRepository
    {
        public OrderRepository(ApplicationDbContext context) : base(context)
        {
        }
        public async Task<IEnumerable<Order>> GetListOrdersByCustomerId(Guid customerId)
        {
            return await GetAllAsync(o => o.UserID == customerId && !o.IsDeleted);
        }
        public async Task<string> GetOrderStatus(Guid orderId)
        {
            var order = await GetAsync(o => o.OrderID == orderId && !o.IsDeleted);
            if (order == null)
            {
                return "Not Found";
            }
            return order.IsActive ? "Active" : "Inactive";
        }
        public async Task<decimal> CalculateTotalAmount(Guid orderId)
        {
            return await _context.OrderDetails
                                 .Where(od => od.OrderID == orderId)
                                 .SumAsync(od => od.UnitPrice * od.Quantity);
        }
    }
}
