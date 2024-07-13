using System.ComponentModel.DataAnnotations;

namespace Hufniture_API.Models
{
    public class FurnitureCategory
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        [MaxLength(100)]
        public required string Name { get; set; }
        public required string CategoryIcon { get; set; }
        public ICollection<FurnitureProduct> FurnitureProducts { get; set; }
        public ICollection<FurnitureType> FurnitureTypes { get; set; }
    }
}
