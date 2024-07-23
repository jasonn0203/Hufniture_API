using System.ComponentModel.DataAnnotations;

namespace Hufniture_API.ViewModel
{
    public class ProductFilterVM
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string ImageURL { get; set; }
        public decimal Price { get; set; }
        public string CategoryName { get; set; }
        public string ColorName { get; set; }

    }
}
