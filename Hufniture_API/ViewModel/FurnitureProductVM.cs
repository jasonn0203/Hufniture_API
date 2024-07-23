using System.ComponentModel.DataAnnotations;

namespace Hufniture_API.ViewModel
{
    public class FurnitureProductVM
    {
        [Required(ErrorMessage = "Tên không được để trống")]
        [MaxLength(50)]
        public required string Name { get; set; }

        [Required(ErrorMessage = "Mô tả SP không được để trống")]
        [MaxLength(400)]
        public required string Description { get; set; }

        [Required(ErrorMessage = "Hình ảnh không được để trống")]
        public required string ImageURL { get; set; }

        [Required(ErrorMessage = "Giá không được để trống")]
        [Range(1000.0, 1000000000.0, ErrorMessage = "Giá phải từ 1,000 VND tới 1,000,000,000 VND.")]
        [DataType(DataType.Currency)]
        public required decimal Price { get; set; }


        public Guid ColorId { get; set; }
        public Guid FurnitureCategoryId { get; set; }
        public Guid FurnitureTypeId { get; set; }
    }
}
