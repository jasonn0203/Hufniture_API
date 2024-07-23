using Hufniture_API.Models;
using Hufniture_API.UnitOfWork;
using Hufniture_API.ViewModel;
using Microsoft.EntityFrameworkCore;

namespace Hufniture_API.Services.FurnitureCategoryService
{
    public class FurnitureCategoryService : IFurnitureCategoryService
    {

        public IUnitOfWork _unitOfWork;
        public FurnitureCategoryService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task AddCategory(FurnitureCategoryVM model)
        {
            var category = new FurnitureCategory
            {

                Name = model.Name,
                CategoryIcon = model.CategoryIcon,
            };

            await _unitOfWork.FurnitureCategoryRepository.AddAsync(category);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task DeleteCategory(Guid id)
        {
            var category = await _unitOfWork.FurnitureCategoryRepository.GetByIdAsync(id);
            if (category == null)
            {
                throw new Exception($"Không tồn tại {id} này.");
            }

            await _unitOfWork.FurnitureCategoryRepository.DeleteAsync(id);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<IEnumerable<FurnitureCategory>> GetAll()
        {
            var categories = await _unitOfWork.FurnitureCategoryRepository.GetAllAsync();
            return categories ?? new List<FurnitureCategory>();
        }


        public async Task<FurnitureCategoryDetailsVM> GetFurnitureCategoryWithDetailsById(Guid categoryId)
        {
            var category = await _unitOfWork.FurnitureCategoryRepository
              .FindByCondition(c => c.Id == categoryId)
              .Include(c => c.FurnitureTypes)
                  .ThenInclude(ft => ft.FurnitureProducts)
              .AsNoTracking()
              .FirstOrDefaultAsync();

            if (category == null)
            {
                throw new Exception("Category not found.");
            }

            var categoryDetails = new FurnitureCategoryDetailsVM
            {
                CategoryId = category.Id,
                CategoryName = category.Name,
                FurnitureTypes = category.FurnitureTypes.Select(ft => new FurnitureTypeDetailsVM
                {
                    TypeName = ft.Name,
                    Products = ft.FurnitureProducts.Select(fp => new ProductResponseVM
                    {
                       Id = fp.Id,
                        Name = fp.Name,
                        Description = fp.Description,
                        ImageURL = fp.ImageURL,
                        Price = fp.Price,
                        ColorId = fp.ColorId,
                        FurnitureCategoryId = fp.FurnitureCategoryId,
                        FurnitureTypeId = fp.FurnitureTypeId
                    }).ToList()
                }).ToList()
            };

            return categoryDetails;
        }


        public async Task<IEnumerable<FurnitureType>> GetFurnitureTypesListById(Guid categoryId)
        {
            return await _unitOfWork.FurnitureTypeRepository.FindByCondition(ft => ft.FurnitureCategoryId == categoryId).AsNoTracking().ToListAsync();
        }


        public async Task UpdateCategory(Guid id, FurnitureCategoryVM model)
        {
            var category = await _unitOfWork.FurnitureCategoryRepository.GetByIdAsync(id);
            if (category == null)
            {
                throw new Exception($"Không tồn tại {id} này.");
            }

            category.Name = model.Name;
            category.CategoryIcon = model.CategoryIcon;


            await _unitOfWork.FurnitureCategoryRepository.UpdateAsync(category);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}
