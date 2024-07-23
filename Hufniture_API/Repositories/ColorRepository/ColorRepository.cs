using Hufniture_API.Data;
using Hufniture_API.Models;

namespace Hufniture_API.Repositories.ColorRepository
{
    public class ColorRepository : GenericRepository<Color>, IColorRepository
    {
        public ColorRepository(HufnitureDbContext context) : base(context)
        {
        }
    }
}
