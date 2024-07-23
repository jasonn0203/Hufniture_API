namespace Hufniture_API.ViewModel
{
    public class FurnitureCategoryDetailsVM
    {
        public Guid CategoryId { get; set; }
        public string CategoryName { get; set; }
        public List<FurnitureTypeDetailsVM> FurnitureTypes { get; set; }
    }
}
