using DataAccess.DataContext;
using DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repositories
{
    public class OrderDetailRepository : Repository<OrderDetail>, IOrderDetailRepository
    {
        public OrderDetailRepository(ApplicationDbContext context) : base(context)
        {
        }
        public async Task<IEnumerable<OrderDetail>> GetOrderDetailsByOrderId(Guid orderId)
        {
            return await GetAllAsync(od => od.OrderID == orderId && !od.IsDeleted);
        }
    }
}
