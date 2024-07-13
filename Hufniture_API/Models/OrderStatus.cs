using Hufniture_API.Enums;
using System.ComponentModel.DataAnnotations;

namespace Hufniture_API.Models
{
    public class OrderStatus
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid OrderId { get; set; }
        public Order Order { get; set; }

        [EnumDataType(typeof(OrderStatusType))]
        public OrderStatusType Status { get; set; }
        public DateTime StatusChangedDate { get; set; }
    }

    public enum OrderStatusType
    {
        Confirmed,
        InDelivery,
        Delivered
    }

}
