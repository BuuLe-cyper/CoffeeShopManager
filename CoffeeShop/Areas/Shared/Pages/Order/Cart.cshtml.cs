using AutoMapper;
using BussinessObjects.DTOs;
using BussinessObjects.Services;
using CoffeeShop.ViewModels;
using DataAccess.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore.SqlServer.Query.Internal;
using Newtonsoft.Json;

namespace CoffeeShop.Areas.Shared.Pages.Order
{
    public class CartModel : PageModel
    {
        private readonly IOrderService _orderService;
        private readonly IOrderDetailService _orderDetailService;
        private readonly IProductSizesService _productSizesService;
        private readonly IMapper _mapper;

        public CartModel(IOrderService orderService, IMapper mapper, IOrderDetailService orderDetailService, IProductSizesService productSizesService)
        {
            _orderService = orderService;
            _orderDetailService = orderDetailService;
            _mapper = mapper;
            _productSizesService = productSizesService;
        }

        public IEnumerable<OrderDetailVM> OrderDetails { get; set; } = default!;
        public IEnumerable<OrderVM> Orders { get; set; } = default!;

        public async Task<IActionResult> OnPostCheckoutAsync(string cartData, string? paymentMethod)
        {
            if (string.IsNullOrEmpty(cartData))
            {
                return BadRequest("Cart data is empty or invalid.");
            }

            try
            {
                var cart = JsonConvert.DeserializeObject<CartData>(cartData);

                if (cart == null)
                {
                    return BadRequest("Unable to deserialize the cart data.");
                }
                // Create Order
                var orderVM = new OrderVM
                {
                    OrderId = Guid.NewGuid(),
                    UserID = cart.UserId,
                    OrderDate = DateTime.Now,
                    PaymentMethod = paymentMethod,
                    TotalAmount = cart.TotalAmount,
                    TableID = 1,
                };
                var order = _mapper.Map<OrderDTO>(orderVM);
                await _orderService.CreateOrder(order);

                if (!string.IsNullOrEmpty(paymentMethod) && paymentMethod.Equals("Cash"))
                {
                    // Process for cash
                    foreach (var item in cart.CartItems)
                    {
                        var productSizeViewDto = await _productSizesService.GetProductName(item.ProductName);
                        var orderDetail = new OrderDetailVM
                        {
                            OrderID = order.OrderId,
                            ProductSizeID = (int)(productSizeViewDto.FirstOrDefault(it => it.SizeID == item.SizeID)?.ProductSizeID),
                            Quantity = item.Quantity,
                            UnitPrice = item.UnitPrice,
                            Discount = item.Discount,
                            ProductName = item.ProductName
                        };
                        var orderDetailDTO = _mapper.Map<OrderDetailDTO>(orderDetail);
                        await _orderDetailService.AddOrderDetail(orderDetailDTO);
                    }
                }
                else
                {
                    // Process for bank trasfer
                }
                // Process the cart data here
                // Save Data
         
                // Return success response
                return new JsonResult(new { success = true });
            }
            catch (JsonException ex)
            {
                // Handle the error if JSON deserialization fails
                return BadRequest($"Failed to deserialize cart data: {ex.Message}");
            }

        }
    }
}
