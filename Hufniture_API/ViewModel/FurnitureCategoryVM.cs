using System.ComponentModel.DataAnnotations;

namespace Hufniture_API.ViewModel
{
    public class FurnitureCategoryVM
    {
        [Required(ErrorMessage = "Tê danh mục không được để trống")]
        public required string Name { get; set; }
        [Required(ErrorMessage = "Icon không được để trống")]
        public required string CategoryIcon { get; set; }
    }
}
