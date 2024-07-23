using Hufniture_API.Models;
using Hufniture_API.UnitOfWork;
using Hufniture_API.ViewModel;

namespace Hufniture_API.Services.FurnitureTypeService
{
    public class FurnitureTypeService : IFurnitureTypeService
    {
        public IUnitOfWork _unitOfWork;
        public FurnitureTypeService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task AddType(FurnitureTypeVM model)
        {
            var type = new FurnitureType
            {

                FurnitureCategoryId = model.FurnitureCategoryId,
                Name = model.Name

            };

            await _unitOfWork.FurnitureTypeRepository.AddAsync(type);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task DeleteType(Guid id)
        {
            var type = await _unitOfWork.FurnitureTypeRepository.GetByIdAsync(id);
            if (type == null)
            {
                throw new Exception($"Không tồn tại {id} này.");
            }

            await _unitOfWork.FurnitureTypeRepository.DeleteAsync(id);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<IEnumerable<FurnitureType>> GetAll()
        {
            var types = await _unitOfWork.FurnitureTypeRepository.GetAllAsync();
            return types ?? new List<FurnitureType>();
        }


        public async Task UpdateType(Guid id, FurnitureTypeVM model)
        {
            var type = await _unitOfWork.FurnitureTypeRepository.GetByIdAsync(id);
            if (type == null)
            {
                throw new Exception($"Không tồn tại {id} này.");
            }

            type.FurnitureCategoryId = model.FurnitureCategoryId;
            type.Name = model.Name;

            await _unitOfWork.FurnitureTypeRepository.UpdateAsync(type);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}
