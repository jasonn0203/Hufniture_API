using System.ComponentModel.DataAnnotations;

namespace Hufniture_API.ViewModel
{
    public class OrderItemVM
    {
        [Required(ErrorMessage = "Prod ID không được để trống")]
        public Guid ProductId { get; set; }
        [Required(ErrorMessage = "Số lượng SP không được để trống")]
        [Range(1, int.MaxValue, ErrorMessage = "Số lượng phải lớn hơn 0")]
        public int Quantity { get; set; }
    }
}
