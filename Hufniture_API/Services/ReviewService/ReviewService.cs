using Hufniture_API.Models;
using Hufniture_API.UnitOfWork;
using Hufniture_API.ViewModel;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Hufniture_API.Services.ReviewService
{
    public class ReviewService : IReviewService
    {
        public IUnitOfWork _unitOfWork;
        public ReviewService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task AddReview(ReviewVM model)
        {
            //Check if exists user
            var user = await _unitOfWork.UserRepository.GetByIdStringAsync(model.UserId);
            if (user == null)
            {
                throw new ArgumentException($"User với ID {model.UserId} không tồn tại.");
            }


            //Check if exists prod
            var prod = await _unitOfWork.FurnitureProductRepository.GetByIdAsync(model.FurnitureProductId);
            if (prod == null)
            {
                throw new ArgumentException($"Sản phẩm với ID {model.UserId} không tồn tại.");

            }


            var review = new Review
            {
                FurnitureProductId = model.FurnitureProductId,
                UserId = model.UserId,
                Content = model.Content,
                CreatedAt = model.CreatedAt,

            };
            await _unitOfWork.ReviewRepository.AddAsync(review);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task DeleteReview(Guid id)
        {
            var review = await _unitOfWork.ReviewRepository.GetByIdAsync(id);
            if (review == null)
            {
                throw new Exception($"Không tồn tại bình luận với id : {id} này.");
            }

            await _unitOfWork.ReviewRepository.DeleteAsync(id);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<IEnumerable<ReviewResponseVM>> GetReviewsByProductId(Guid furnitureProductId)
        {

            // Sử dụng FindByCondition để tìm các review và chỉ chọn các thuộc tính cần thiết
            var reviewsQuery = _unitOfWork.ReviewRepository.FindByCondition(r => r.FurnitureProductId == furnitureProductId)
                .Select(r => new ReviewResponseVM
                {
                    Id = r.Id,
                    Content = r.Content,
                    CreatedAt = r.CreatedAt,
                    UserFullName = r.User.FullName,
                    UserId = r.UserId
                });

            // Lấy danh sách các review từ truy vấn
            return await reviewsQuery.AsNoTracking().ToListAsync();
        }


        public async Task<string> GetUserIdByReviewId(Guid reviewId)
        {
            var review = await _unitOfWork.ReviewRepository.GetByIdAsync(reviewId);
            if (review == null)
            {
                throw new Exception($"Không tồn tại bình luận với id : {reviewId} này.");
            }

            return review.UserId;
        }
    }
}
