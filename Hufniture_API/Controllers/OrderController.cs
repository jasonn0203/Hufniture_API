using Hufniture_API.Models;
using Hufniture_API.Services.OrderService;
using Hufniture_API.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hufniture_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;


        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [Authorize]
        [HttpPost("PlaceOrder")]
        public async Task<IActionResult> PlaceOrder([FromBody] PlaceOrderVM orderRequest)
        {
            try
            {

                var orderItems = new List<OrderItem>();
                foreach (var item in orderRequest.Items)
                {
                    orderItems.Add(new OrderItem
                    {
                        FurnitureProductId = item.ProductId,
                        Quantity = item.Quantity
                    });
                }

                var order = await _orderService.PlaceOrderAsync(orderRequest.UserId, orderItems);
                return Ok(order);
            }
            catch (Exception ex)
            {
                // Log the exception (not shown here)
                return StatusCode(500, ex.Message);
            }
        }


        [Authorize]
        [HttpGet("GetOrderById/{orderId}")]
        public async Task<IActionResult> GetOrderById(Guid orderId)
        {
            try
            {
                var order = await _orderService.GetOrderByIdAsync(orderId);
                if (order == null)
                {
                    return NotFound();
                }

                return Ok(order);
            }
            catch (Exception ex)
            {
                // Log the exception (not shown here)
                return StatusCode(500, ex.Message);
            }
        }


        [Authorize]
        [HttpGet("GetOrdersByUserId/{userId}")]
        public async Task<IActionResult> GetOrdersByUserId(string userId)
        {
            try
            {
                var orders = await _orderService.GetOrdersByUserIdAsync(userId);
                if (orders == null)
                {
                    return NotFound();
                }
                return Ok(orders);
            }
            catch (Exception ex)
            {
                // Log the exception (not shown here)
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPut("UpdateOrderStatus/{orderId}")]
        public async Task<IActionResult> UpdateOrderStatus([FromBody] OrderStatusVM updateRequest)
        {
            try
            {
                await _orderService.UpdateOrderStatusAsync(updateRequest.OrderId, updateRequest.NewStatus);
                return Ok();
            }
            catch (Exception ex)
            {
                // Log the exception (not shown here)
                return StatusCode(500, ex.Message);
            }


        }
    }
}
