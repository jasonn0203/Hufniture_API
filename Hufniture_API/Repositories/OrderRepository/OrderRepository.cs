using Hufniture_API.Data;
using Hufniture_API.Models;

namespace Hufniture_API.Repositories.OrderRepository
{
    public class OrderRepository : GenericRepository<Order>, IOrderRepository
    {
        public OrderRepository(HufnitureDbContext context) : base(context)
        {
        }

        public void Update(Order order)
        {
            _context.Orders.Update(order);
        }
    }
}
