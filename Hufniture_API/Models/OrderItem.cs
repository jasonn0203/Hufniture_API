using System.ComponentModel.DataAnnotations;

namespace Hufniture_API.Models
{
    public class OrderItem
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid OrderId { get; set; }
        public Order Order { get; set; }
        public Guid FurnitureProductId { get; set; }
        public FurnitureProduct FurnitureProduct { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Số lượng không được giá trị âm và lớn hơn 0")]
        public int Quantity { get; set; }


    }
}
