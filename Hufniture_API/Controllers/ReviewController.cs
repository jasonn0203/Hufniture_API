using Hufniture_API.Data;
using Hufniture_API.Services.ReviewService;
using Hufniture_API.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hufniture_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReviewController : ControllerBase
    {
        private readonly IReviewService _reviewService;

        public ReviewController(IReviewService reviewService)
        {
            _reviewService = reviewService;
        }


        [Authorize]
        [HttpPost("AddReview")]
        public async Task<ActionResult> AddReview([FromBody] ReviewVM model)
        {
            //Check validate
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors)
                                              .Select(e => e.ErrorMessage)
                                              .ToList();
                return BadRequest(new ApiResponse { Status = "Error", Message = string.Join(", ", errors) });
            }

            try
            {
                await _reviewService.AddReview(model);
                return Ok(new ApiResponse { Message = "Tạo thành công", Status = "Success" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Lỗi: {ex.Message}");
            }
        }

        [Authorize]
        [HttpDelete("DeleteReview/{id}")]
        public async Task<ActionResult> DeleteReview(Guid id)
        {
            try
            {
                await _reviewService.DeleteReview(id);
                return Ok(new ApiResponse { Message = $"Bình luận với ID {id} đã được xóa", Status = "Success" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Lỗi: {ex.Message}");
            }
        }

        [HttpGet("GetReviewsByProductId/{productId}")]
        public async Task<IActionResult> GetReviewsByProductId(Guid productId)
        {
            var reviews = await _reviewService.GetReviewsByProductId(productId);
            if (reviews == null || !reviews.Any())
            {
                return NotFound($"Không có dánh giá nào từ SP có ID : {productId}.");
            }
            var response = new
            {
                results = reviews
            };

            return Ok(response);
        }


    }
}
