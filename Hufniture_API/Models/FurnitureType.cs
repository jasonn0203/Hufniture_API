using System.ComponentModel.DataAnnotations;

namespace Hufniture_API.Models
{
    public class FurnitureType
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        [MaxLength(100)]
        public required string Name { get; set; }
        public Guid FurnitureCategoryId { get; set; }
        public FurnitureCategory FurnitureCategory { get; set; }
        public ICollection<FurniturePrProductVM> FurnitureProducts { get; set; }
    }
}
