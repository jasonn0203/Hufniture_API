using System.ComponentModel.DataAnnotations;

namespace Hufniture_API.Models
{
    public class FurnitureProduct
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        [MaxLength(50)]
        public required string Name { get; set; }

        [MaxLength(400)]
        public required string Description { get; set; }

        public required string ImageURL { get; set; }

        [Range(1000.0, 1000000000.0, ErrorMessage = "Giá phải từ 1,000 VND tới 1,000,000,000 VND.")]
        [DataType(DataType.Currency)]
        public required decimal Price { get; set; }



        public Guid ColorId { get; set; }
        public Color Color { get; set; }
        public Guid FurnitureCategoryId { get; set; }
        public FurnitureCategory FurnitureCategory { get; set; }
        public Guid FurnitureTypeId { get; set; }
        public FurnitureType FurnitureType { get; set; }
        public ICollection<Review> Reviews { get; set; }
    }
}
