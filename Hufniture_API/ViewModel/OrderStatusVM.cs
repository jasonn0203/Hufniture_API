using Hufniture_API.Models;
using System.ComponentModel.DataAnnotations;

namespace Hufniture_API.ViewModel
{
    public class OrderStatusVM
    {
        public Guid OrderId { get; set; }
        public OrderStatusType NewStatus { get; set; }

    }
}
