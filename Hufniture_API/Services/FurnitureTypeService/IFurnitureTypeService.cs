using Hufniture_API.Models;
using Hufniture_API.ViewModel;

namespace Hufniture_API.Services.FurnitureTypeService
{
    public interface IFurnitureTypeService
    {
        Task<IEnumerable<FurnitureType>> GetAll();
        Task AddType(FurnitureTypeVM model);
        Task DeleteType(Guid id);
        Task UpdateType(Guid id, FurnitureTypeVM model);
    }
}
