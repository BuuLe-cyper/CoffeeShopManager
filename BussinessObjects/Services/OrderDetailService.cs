﻿using AutoMapper;
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
        private readonly IOrderRepository _orderRepository;
        private readonly IMapper _mapper;
        public OrderDetailService(IOrderDetailRepository orderDetailRepository, IMapper mapper, IOrderRepository orderRepository)
        {
            _orderDetailRepository = orderDetailRepository;
            _orderRepository = orderRepository;
            _mapper = mapper;
        }
		public async Task<IEnumerable<OrderDetailDTO>> GetOrderDetailsByOrderId(Guid orderId)
		{
			var orderDetails = await _orderDetailRepository.GetAllAsync(
				it => it.OrderID == orderId && !it.IsDeleted,
				includeProperties: "ProductSize.Product,ProductSize.Size"
			);

			var orderDetailDTOs = orderDetails.Select(od => new OrderDetailDTO
			{
				OrderDetailId = od.OrderDetailID,
				UnitPrice = od.UnitPrice,
				Quantity = od.Quantity,
				Discount = od.Discount,
				OrderID = od.OrderID,
				ProductSizeID = od.ProductSizeID,
				ProductName = od.ProductSize?.Product?.ProductName ?? "Unknown",
				SizeName = od.ProductSize?.Size?.SizeName ?? "Unknown"
			});

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
