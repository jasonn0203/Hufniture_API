using Hufniture_API.Models;

namespace Hufniture_API.Services.OrderService
{
    public interface IOrderService
    {
        //Các nghiệp vụ liên quan đến đặt hàng của user và thao tác liên quan
        Task<Order> PlaceOrderAsync(string userId, List<OrderItem> orderItems);
        Task<Order?> GetOrderByIdAsync(Guid orderId);
        //Task<IEnumerable<Order>> GetOrdersByUserIdAsync(string userId);
        Task UpdateOrderStatusAsync(Guid orderId, OrderStatusType newStatus);

        //Task<List<Order>> GetOrdersByUserIdAsync(string userId);
        Task<List<Order>> GetOrdersByUserIdAsync(string userId);

    }
}
