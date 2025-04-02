using DataAccess.DataContext;
using DataAccess.Models;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Repositories
{
    public class OrderDetailRepository : Repository<OrderDetail>, IOrderDetailRepository
    {
        public OrderDetailRepository(ApplicationDbContext context) : base(context)
        {
        }

		public async Task<IEnumerable<OrderDetail>> GetOrderDetailsByOrderIdAsync(Guid orderId)
		{
			return await _context.OrderDetails
				.Where(od => od.OrderID == orderId)
				.Include(od => od.ProductSize)
					.ThenInclude(ps => ps.Product) 
				.Include(od => od.ProductSize)
					.ThenInclude(ps => ps.Size)   
				.ToListAsync();
		}
	}
}
