using Hufniture_API.Data;
using Hufniture_API.Models;

namespace Hufniture_API.Repositories.FurnitureTypeRepository
{
    public class FurnitureTypeRepository : GenericRepository<FurnitureType>, IFurnitureTypeRepository
    {
        public FurnitureTypeRepository(HufnitureDbContext context) : base(context)
        {
        }
    }
}
