using System.ComponentModel.DataAnnotations;

namespace Hufniture_API.ViewModel
{
    public class FurnitureTypeVM
    {
        [Required(ErrorMessage = "Tên loại nội thất không được để trống")]
        public required string Name { get; set; }
        public Guid FurnitureCategoryId { get; set; }
    }
}
