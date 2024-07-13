using Hufniture_API.Data;
using System.ComponentModel.DataAnnotations;

namespace Hufniture_API.Models
{
    public class Order
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string UserId { get; set; }
        public AppUser User { get; set; }

        [DataType(DataType.Date)]
        public DateTime OrderDate { get; set; }
        public ICollection<OrderItem> OrderItems { get; set; }
        public ICollection<OrderStatus> OrderStatuses { get; set; }
    }
}
