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
        public async Task<decimal> CalculateTotalAmount(Guid orderId)
        {
            return await _context.OrderDetails
                                 .Where(od => od.OrderID == orderId)
                                 .SumAsync(od => od.UnitPrice * od.Quantity);
        }
        public async Task<IEnumerable<Order>> GetOrdersByOrderId(Guid orderId)
        {
            return await _context.Orders
                                    .Where(order => order.OrderID == orderId)
                                    .ToListAsync();
        }

		public async Task<bool> CheckIfUserExists(Guid userId)
		{
			if (userId == Guid.Empty) return true;

			return await _context.Users.AnyAsync(u => u.UserID == userId);
		}

	}
}
