using System.ComponentModel.DataAnnotations;

namespace Hufniture_API.ViewModel
{
    public class ProductColorVM
    {
        [Required(ErrorMessage = "Tên màu không được để trống")]
        [MaxLength(30)]
        public required string Name { get; set; }
    }
}
