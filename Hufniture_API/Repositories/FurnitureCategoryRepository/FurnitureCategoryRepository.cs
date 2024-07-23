using Hufniture_API.Data;
using Hufniture_API.Models;
using Hufniture_API.ViewModel;

namespace Hufniture_API.Repositories.FurnitureCategoryRepository
{
    public class FurnitureCategoryRepository : GenericRepository<FurnitureCategory>, IFurnitureCategoryRepository
    {
        public FurnitureCategoryRepository(HufnitureDbContext context) : base(context)
        {
        }

  
    }
}
