using Hufniture_API.Models;
using Hufniture_API.UnitOfWork;
using Microsoft.EntityFrameworkCore;

namespace Hufniture_API.Services.OrderService
{
    public class OrderService : IOrderService
    {
        public IUnitOfWork _unitOfWork;
        public OrderService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Order?> GetOrderByIdAsync(Guid orderId)
        {
            return await _unitOfWork.OrderRepository.GetByIdAsync(orderId);
        }

        public async Task<List<Order>> GetOrdersByUserIdAsync(string userId) // Implement phương thức mới
        {
            var orders = await _unitOfWork.OrderRepository
                .FindByCondition(o => o.UserId == userId)
                .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.FurnitureProduct)
                .Include(o => o.OrderStatuses)
                .ToListAsync();
            return orders;
        }



        public async Task<Order> PlaceOrderAsync(string userId, List<OrderItem> orderItems)
        {
            var order = new Order
            {
                UserId = userId,
                OrderDate = DateTime.Now,
                OrderItems = orderItems,
                OrderStatuses = new List<OrderStatus>
            {
                new OrderStatus
                {
                    Status = OrderStatusType.Confirmed,
                    StatusChangedDate = DateTime.Now
                }
            }
            };

            await _unitOfWork.OrderRepository.AddAsync(order);
            await _unitOfWork.SaveChangesAsync();

            return order;
        }


        public async Task UpdateOrderStatusAsync(Guid orderId, OrderStatusType newStatus)
        {
            var order = await _unitOfWork.OrderRepository.GetByIdAsync(orderId);
            if (order == null)
            {
                throw new Exception("Order not found");
            }

            if (order.OrderStatuses == null)
            {
                order.OrderStatuses = new List<OrderStatus>();
            }

            var orderStatus = new OrderStatus
            {
                OrderId = orderId,
                Status = newStatus,
                StatusChangedDate = DateTime.Now
            };

            order.OrderStatuses.Add(orderStatus);
            _unitOfWork.OrderRepository.Update(order);
            await _unitOfWork.SaveChangesAsync();
        }





    }
}
