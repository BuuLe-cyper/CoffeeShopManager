using BussinessObjects.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BussinessObjects.Services
{
    public interface IOrderService
    {
        public Task CreateOrder(OrderDTO orderDTO);
        public Task<IEnumerable<OrderDTO>> GetListOrdersByCustomerId(Guid customerId);
        public Task<bool?> GetOrderStatus(Guid orderId);
        public Task<decimal> CalculateTotalAmount(Guid orderId);
    }
}
