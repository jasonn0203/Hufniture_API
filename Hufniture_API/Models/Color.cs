using System.ComponentModel.DataAnnotations;

namespace Hufniture_API.Models
{
    public class Color
    {
        public Guid Id { get; set; } = Guid.NewGuid();

   
        [MaxLength(30)]
        public required string Name { get; set; }


        public required ICollection<FurniturePrProductVM> FurnitureProducts { get; set; }


    }
}
