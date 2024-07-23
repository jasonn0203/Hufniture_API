using Hufniture_API.Data;
using Hufniture_API.Models;
using System.ComponentModel.DataAnnotations;

namespace Hufniture_API.ViewModel
{
    public class ReviewVM
    {
        [Required(ErrorMessage = "Prod ID khôngđược để trống")]
        public Guid FurnitureProductId { get; set; }

        [Required(ErrorMessage = "UserId khôngđược để trống")]
        public string UserId { get; set; }

        [Required(ErrorMessage = "Nội dung không được để trống")]
        [MaxLength(1000)]
        public string Content { get; set; }
        [DataType(DataType.Date)]
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}
