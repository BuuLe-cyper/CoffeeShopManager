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

        public async Task<decimal> CalculateTotalAmount(Guid orderId)
        {
            return await _orderRepository.CalculateTotalAmount(orderId);
        }

        public async Task<IEnumerable<OrderDTO>> GetListOrdersByCustomerId(Guid customerId)
        {
            return _mapper.Map<IEnumerable<OrderDTO>>(await _orderRepository.GetListOrdersByCustomerId(customerId));
        }

        public async Task<string> GetOrderStatus(Guid orderId)
        {
            return await _orderRepository.GetOrderStatus(orderId);
        }
    }
}
