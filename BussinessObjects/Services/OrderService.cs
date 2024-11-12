using AutoMapper;
using BussinessObjects.DTOs;
using DataAccess.Models;
using DataAccess.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BussinessObjects.Services
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IMapper _mapper;
        public OrderService(IOrderRepository orderRepository, IMapper mapper)
        {
            _orderRepository = orderRepository;
            _mapper = mapper;
        }
        public async Task CreateOrder(OrderDTO orderDTO)
        {
            ArgumentNullException.ThrowIfNull(orderDTO);
            try
            {
                var order = _mapper.Map<Order>(orderDTO);
                await _orderRepository.CreateAsync(order);
            }
            catch (Exception)
            {
                throw;
            }
        }
        public async Task<IEnumerable<OrderDTO>> GetOrderByOrderId(Guid orderId)
        {
            var orders = await _orderRepository.GetOrdersByOrderId(orderId);
            return _mapper.Map<IEnumerable<OrderDTO>>(orders); 
        }

        public async Task<decimal> CalculateTotalAmount(Guid orderId)
        {
            return await _orderRepository.CalculateTotalAmount(orderId);
        }

        public async Task<IEnumerable<OrderDTO>> GetListOrdersByCustomerId(Guid customerId)
        {
            return _mapper.Map<IEnumerable<OrderDTO>>(await _orderRepository.
                GetAllAsync(o => o.UserID == customerId && !o.IsDeleted));
        }

        public async Task<bool?> GetOrderStatus(Guid orderId)
        {
            var order = await _orderRepository.GetAsync(o => o.OrderID == orderId && !o.IsDeleted);

            if (order == null)
            {
                return null; 
            }

            return order.IsActive; 
        }
        public async Task<IEnumerable<OrderDTO>> GetAllOrder()
        {
            var products = await _orderRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<OrderDTO>>(products);
        }
    }
}
