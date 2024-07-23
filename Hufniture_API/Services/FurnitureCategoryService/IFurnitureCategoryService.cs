using Hufniture_API.Models;
using Hufniture_API.ViewModel;

namespace Hufniture_API.Services.FurnitureCategoryService
{
    public interface IFurnitureCategoryService
    {
        Task<IEnumerable<FurnitureCategory>> GetAll();
        Task<IEnumerable<FurnitureType>> GetFurnitureTypesListById(Guid categoryId);
        Task<FurnitureCategoryDetailsVM> GetFurnitureCategoryWithDetailsById(Guid categoryId);

        Task AddCategory(FurnitureCategoryVM model);
        Task DeleteCategory(Guid id);
        Task UpdateCategory(Guid id, FurnitureCategoryVM model);



    }
}
