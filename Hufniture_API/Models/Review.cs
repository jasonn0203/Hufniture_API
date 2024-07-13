using Hufniture_API.Data;
using System.ComponentModel.DataAnnotations;

namespace Hufniture_API.Models
{
    public class Review
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid FurnitureProductId { get; set; }
        public FurnitureProduct FurnitureProduct { get; set; }
        public string UserId { get; set; }
        public AppUser User { get; set; }

        [MaxLength(1000)]
        public string Content { get; set; }
        [DataType(DataType.Date)]
        public DateTime CreatedAt { get; set; }
    }
}
