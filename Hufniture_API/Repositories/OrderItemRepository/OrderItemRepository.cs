using Hufniture_API.Data;
using Hufniture_API.Models;

namespace Hufniture_API.Repositories.OrderItemRepository
{
    public class OrderItemRepository : GenericRepository<OrderItem>, IOrderItemRepository
    {
        public OrderItemRepository(HufnitureDbContext context) : base(context)
        {
        }
    }
}
