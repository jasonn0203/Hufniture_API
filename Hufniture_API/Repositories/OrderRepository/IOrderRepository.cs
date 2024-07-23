using Hufniture_API.Models;
using Hufniture_API.Repositories.Interfaces;

namespace Hufniture_API.Repositories.OrderRepository
{
    public interface IOrderRepository : IGenericRepository<Order>
    {
        void Update(Order order);

    }
}
