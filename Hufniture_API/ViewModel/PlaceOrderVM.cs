using System.ComponentModel.DataAnnotations;

namespace Hufniture_API.ViewModel
{
    public class PlaceOrderVM
    {
        [Required(ErrorMessage = "User ID không được để trống")]
        public string UserId { get; set; }

        [Required(ErrorMessage = "Các sản phẩm không được để trống")]
        public List<OrderItemVM> Items { get; set; }
    }
}
