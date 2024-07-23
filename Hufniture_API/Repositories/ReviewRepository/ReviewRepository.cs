using Hufniture_API.Data;
using Hufniture_API.Models;

namespace Hufniture_API.Repositories.ReviewRepository
{
    public class ReviewRepository : GenericRepository<Review>, IReviewRepository
    {
        public ReviewRepository(HufnitureDbContext context) : base(context)
        {
        }
    }
}
