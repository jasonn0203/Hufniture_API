using Hufniture_API.Models;
using Hufniture_API.ViewModel;

namespace Hufniture_API.Services.ReviewService
{
    public interface IReviewService
    {
        Task AddReview(ReviewVM model);
        Task DeleteReview(Guid id);

        Task<IEnumerable<ReviewResponseVM>> GetReviewsByProductId(Guid furnitureProductId);
        Task<string> GetUserIdByReviewId(Guid reviewId);
    }
}
