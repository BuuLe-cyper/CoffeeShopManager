using DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repositories
{
    public interface IOrderRepository : IRepository<Order>
    {
        public Task<decimal> CalculateTotalAmount(Guid orderId);
        public Task<IEnumerable<Order>> GetOrdersByOrderId(Guid orderId);
    }
}
