using AutoMapper;
using BussinessObjects.DTOs;
using BussinessObjects.Services;
using CoffeeShop.ViewModels;
using DataAccess.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore.SqlServer.Query.Internal;
using Net.payOS.Types;
using Net.payOS;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;

namespace CoffeeShop.Areas.Shared.Pages.Order
{
    [AllowAnonymous]
    public class CartModel : PageModel
    {
        private readonly IOrderService _orderService;
        private readonly IOrderDetailService _orderDetailService;
        private readonly IProductSizesService _productSizesService;
        private readonly IUserService _userService;
        private readonly IMapper _mapper;
        private readonly PayOS _payOS;

        public CartModel(IOrderService orderService, IMapper mapper, IOrderDetailService orderDetailService, IProductSizesService productSizesService, PayOS payOS, IUserService userService)
        {
            _orderService = orderService;
            _orderDetailService = orderDetailService;
            _mapper = mapper;
            _productSizesService = productSizesService;
            _payOS = payOS;
            _userService = userService;
        }

        public IEnumerable<OrderDetailVM> OrderDetails { get; set; } = default!;
        public IEnumerable<OrderVM> Orders { get; set; } = default!;
        public string TableId { get; set; }

        public async Task OnGetAsync(string? tableId)
        {
         
            TableId = tableId;
        }

        public async Task<IActionResult> OnPostCheckoutAsync(string cartData, string? paymentMethod, CreatePaymentLinkRequest body)
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

                Guid userId = (Guid)(cart.UserId != Guid.Empty ? cart.UserId : await _userService.GetGuestUserIdAsync());

                var orderVM = new OrderVM
                {
                    OrderId = Guid.NewGuid(),
                    //UserID = cart.UserId,
                    UserID = userId,
                    OrderDate = DateTime.Now,
                    PaymentMethod = paymentMethod,
                    TotalAmount = cart.TotalAmount,
                    TableID = (int)cart.TableId,
                };
                var order = _mapper.Map<OrderDTO>(orderVM);
                await _orderService.CreateOrder(order);

                Guid orderId = order.OrderId;
                //Guid userId = cart.UserId;
                HttpContext.Session.SetString("CurrentOrderId", orderId.ToString());
                HttpContext.Session.SetString("UserId", userId.ToString());

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

                    if (!String.IsNullOrEmpty(paymentMethod) && paymentMethod.Equals("BankTransfer"))
                    {
                        body.UserInfor = cart.UserId.ToString();
                        body.TotalPrice = cart.TotalAmount;
                    try
                    {
                            int orderCode = int.Parse(DateTimeOffset.Now.ToString("ffffff"));
                            ItemData item = new ItemData(body.UserInfor, 1, (int)Math.Round(cart.TotalAmount));
                            List<ItemData> items = new List<ItemData> { item };
                            PaymentData paymentData = new PaymentData(orderCode, (int)body.TotalPrice, body.Description, items, body.cancelUrl, body.returnUrl);

                            CreatePaymentResult paymentResult = await _payOS.createPaymentLink(paymentData);

                            return Redirect(paymentResult.checkoutUrl);
                        }
                        catch (System.Exception exception)
                        {
                            return Content("Error Payment Link");
                        }
                    }
                 return new JsonResult(new { success = true });
            }
            catch (JsonException ex)
            {
                return BadRequest($"Failed to deserialize cart data: {ex.Message}");
            }

        }
    }
}
