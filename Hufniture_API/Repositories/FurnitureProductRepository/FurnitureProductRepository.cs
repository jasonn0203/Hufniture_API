using Hufniture_API.Data;
using Hufniture_API.Models;
using Hufniture_API.Repositories.FurnitureTypeRepository;

namespace Hufniture_API.Repositories.FurnitureProductRepository
{
    public class FurnitureProductRepository : GenericRepository<FurniturePrProductVM>, IFurnitureProductRepository
    {
        public FurnitureProductRepository(HufnitureDbContext context) : base(context)
        {
        }
    }
}
