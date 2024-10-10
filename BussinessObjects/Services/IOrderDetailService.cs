using BussinessObjects.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BussinessObjects.Services
{
    public interface IOrderDetailService
    {
        public Task<IEnumerable<OrderDetailDTO>> GetOrderDetailsByOrderId(Guid orderId);
        public Task<bool> AddOrderDetail(OrderDetailDTO orderDetailDTO);
        public Task<bool> UpdateOrderDetail(OrderDetailDTO orderDetailDTO);
    }
}
