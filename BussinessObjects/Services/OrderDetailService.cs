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
    public class OrderDetailService : IOrderDetailService
    {
        private readonly IOrderDetailRepository _orderDetailRepository;
        private readonly IMapper _mapper;
        public OrderDetailService(IOrderDetailRepository orderDetailRepository, IMapper mapper)
        {
            _orderDetailRepository = orderDetailRepository;
            _mapper = mapper;
        }
        public async Task<IEnumerable<OrderDetailDTO>> GetOrderDetailsByOrderId(Guid orderId)
        {
            var orderDetail = await _orderDetailRepository.GetOrderDetailsByOrderId(orderId);
            var orderDetailDTOs = _mapper.Map<IEnumerable<OrderDetailDTO>>(orderDetail);
            return orderDetailDTOs;
        }
        public async Task<bool> AddOrderDetail(OrderDetailDTO orderDetailDTO)
        {
            if (orderDetailDTO == null)
                throw new ArgumentNullException(nameof(orderDetailDTO));
            try
            {
                var orderDetail = _mapper.Map<OrderDetail>(orderDetailDTO);
                await _orderDetailRepository.CreateAsync(orderDetail);

                return true;
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred", ex);
            }
        }
        public async Task<bool> UpdateOrderDetail(OrderDetailDTO orderDetailDTO)
        {
            if (orderDetailDTO == null)
                throw new ArgumentNullException(nameof(orderDetailDTO));
            try
            {
                var orderDetail = _mapper.Map<OrderDetail>(orderDetailDTO);
                await _orderDetailRepository.UpdateAsync(orderDetail);

                return true;
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred", ex);
            }
        }
    }
}
