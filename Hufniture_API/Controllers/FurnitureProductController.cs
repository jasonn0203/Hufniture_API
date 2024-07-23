using Hufniture_API.Data;
using Hufniture_API.Models;
using Hufniture_API.Services.FurnitureProductService;
using Hufniture_API.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hufniture_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FurnitureProductController : ControllerBase
    {
        private readonly IFurnitureProductService _productService;

        public FurnitureProductController(IFurnitureProductService productService)
        {
            _productService = productService;
        }

        [HttpGet("GetAllFurnitureProducts")]
        public async Task<ActionResult<IEnumerable<FurniturePrProductVM>>> GetFurnitureProducts()
        {
            try
            {
                var products = await _productService.GetAll();
                if (products == null || !products.Any())
                {
                    return NotFound(new ApiResponse { Message = "List sản phẩm hiện đang trống!.", Status = "Success" });
                }
                //Return response datas
                var response = new
                {
                    results = products
                };

                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Lỗi: {ex.Message}");
            }
        }


        [HttpGet("GetRandomFurnitureProducts")]
        public async Task<ActionResult<IEnumerable<FurniturePrProductVM>>> GetRandomFurnitureProducts([FromQuery] int take)
        {
            try
            {
                if (take <= 0)
                {
                    return BadRequest(new ApiResponse { Message = "Số lượng sản phẩm phải lớn hơn 0.", Status = "Error" });
                }

                var products = await _productService.GetRandomProductsAsync(take);
                if (products == null || !products.Any())
                {
                    return NotFound(new ApiResponse { Message = "Không tìm thấy sản phẩm nào.", Status = "Success" });
                }

                // Return response data
                var response = new
                {
                    results = products
                };

                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Lỗi: {ex.Message}");
            }
        }


        [HttpGet("GetFurnitureProductById/{id}")]
        public async Task<ActionResult<FurniturePrProductVM>> GetFurnitureProductById(Guid id)
        {
            try
            {
                var product = await _productService.GetByIdAsync(id);
                if (product == null)
                {
                    return BadRequest(new ApiResponse { Message = $"Không tìm thấy sản phẩm với ID {id}", Status = "Error" });
                }
                var response = new
                {
                    results = product
                };

                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Lỗi: {ex.Message}");
            }
        }


        [HttpGet("GetFilteredProducts")]
        public async Task<IActionResult> GetFilteredProducts(
              [FromQuery] decimal? minPrice,
              [FromQuery] decimal? maxPrice,
              [FromQuery] IEnumerable<Guid> categoryIds,
              [FromQuery] IEnumerable<Guid> colorIds)
        {
            try
            {
                var products = await _productService.GetFilteredProductsAsync(minPrice, maxPrice, categoryIds, colorIds);
                var response = new
                {
                    results = products
                };

                return Ok(response);
            }
            catch (Exception ex)
            {
                // Log exception and return a proper error response
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }


        [HttpGet("GetBestSellingProduct")]
        public async Task<IActionResult> GetBestSellingProduct()
        {
            try
            {
                var product = await _productService.GetBestSellingProductAsync();
                var response = new
                {
                    results = product
                };
                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }


        [Authorize(Roles = UserRoles.Admin)]
        [HttpPost("AddFurnitureProduct")]
        public async Task<ActionResult> AddFurnitureProduct([FromBody] FurnitureProductVM model)
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
                await _productService.AddProduct(model);
                return Ok(new ApiResponse { Message = "Tạo thành công", Status = "Success" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Lỗi: {ex.Message}");
            }
        }


        [Authorize(Roles = UserRoles.Admin)]
        [HttpPut("UpdateFurnitureProduct/{id}")]
        public async Task<ActionResult> UpdateFurnitureProduct(Guid id, [FromBody] FurnitureProductVM model)
        {
            try
            {
                await _productService.UpdateProduct(id, model);
                return Ok(new ApiResponse { Message = $"Sản phẩm với ID {id} đã được cập nhật", Status = "Success" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Lỗi: {ex.Message}");
            }
        }

        [Authorize(Roles = UserRoles.Admin)]
        [HttpDelete("DeleteFurnitureProduct/{id}")]
        public async Task<ActionResult> DeleteFurnitureProduct(Guid id)
        {
            try
            {
                await _productService.DeleteProduct(id);
                return Ok(new ApiResponse { Message = $"Sản phẩm với ID {id} đã được xóa", Status = "Success" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Lỗi: {ex.Message}");
            }
        }

        //------------------------
        [HttpGet("GetAllColors")]
        public async Task<ActionResult<IEnumerable<Color>>> GetAllColors()
        {
            try
            {
                var colors = await _productService.GetAllColors();
                if (colors == null || !colors.Any())
                {
                    return BadRequest(new ApiResponse { Message = "List màu hiện đang trống!.", Status = "Success" });
                }
                return Ok(colors);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Lỗi: {ex.Message}");
            }
        }

        [HttpPost("AddProductColor")]
        public async Task<ActionResult> AddProductColor([FromBody] ProductColorVM model)
        {
            try
            {
                await _productService.AddProductColor(model);
                return Ok(new ApiResponse { Message = "Tạo thành công", Status = "Success" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Lỗi: {ex.Message}");
            }
        }


        [HttpPut("UpdateProductColor/{id}")]
        public async Task<ActionResult> UpdateProductColor(Guid id, [FromBody] ProductColorVM model)
        {
            try
            {
                await _productService.UpdateProductColor(id, model);
                return Ok(new ApiResponse { Message = $"Màu {model.Name} với ID {id} đã được cập nhật", Status = "Success" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Lỗi: {ex.Message}");
            }
        }

        [HttpDelete("DeleteProductColor/{id}")]
        public async Task<ActionResult> DeleteProductColor(Guid id)
        {
            try
            {
                await _productService.DeleteProductColor(id);
                return Ok(new ApiResponse { Message = $"Màu sắc với ID {id} đã được xóa", Status = "Success" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Lỗi: {ex.Message}");
            }
        }
        //------------------------
        [HttpGet("GetReviewListByProdId/{id}")]
        public async Task<ActionResult<Review>> GetReviewListByProdId(Guid id)
        {
            try
            {
                var reviews = await _productService.GetReviewByProdId(id);
                if (reviews == null)
                {
                    return NotFound(new ApiResponse { Message = "Không có đánh gá nào.", Status = "Error" });
                }
                var response = new
                {
                    results = reviews
                };

                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Lỗi : {ex.Message}");
            }
        }
    }
}
