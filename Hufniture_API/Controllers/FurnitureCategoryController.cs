using Hufniture_API.Data;
using Hufniture_API.Models;
using Hufniture_API.Services.FurnitureCategoryService;
using Hufniture_API.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace Hufniture_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FurnitureCategoryController : ControllerBase
    {
        private readonly IFurnitureCategoryService _categoryService;

        public FurnitureCategoryController(IFurnitureCategoryService categoryService)
        {
            _categoryService = categoryService;
        }


        [HttpGet]
        public async Task<ActionResult<IEnumerable<FurnitureCategory>>> GetFurnitureCategories()
        {
            try
            {
                var categories = await _categoryService.GetAll();
                if (categories == null || !categories.Any())
                {
                    return NotFound(new ApiResponse { Message = "Không có danh mục nào được tìm thấy.", Status = "Success" });
                }
                var response = new
                {
                    results = categories
                };

                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Lỗi : {ex.Message}");
            }
        }

        [HttpGet("GetFurnitureTypesListById/{id}")]
        public async Task<ActionResult<FurnitureCategory>> GetFurnitureTypesListById(Guid id)
        {
            try
            {
                var category = await _categoryService.GetFurnitureTypesListById(id);
                if (category == null)
                {
                    return NotFound(new ApiResponse { Message = "Danh mục không được tìm thấy.", Status = "Error" });
                }
                var response = new
                {
                    results = category
                };

                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Lỗi : {ex.Message}");
            }
        }

        [HttpGet("GetFurnitureCategoryDetailsById/{id}")]
        public async Task<ActionResult<FurnitureCategoryDetailsVM>> GetFurnitureCategoryDetailsById(Guid id)
        {
            try
            {
                var categoryDetails = await _categoryService.GetFurnitureCategoryWithDetailsById(id);
                if (categoryDetails == null)
                {
                    return NotFound(new ApiResponse { Message = "Category not found.", Status = "Error" });
                }
                var response = new { results = categoryDetails };
                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error: {ex.Message}");
            }
        }


        [Authorize(Roles = UserRoles.Admin)]
        [HttpPost]
        public async Task<ActionResult> AddFurnitureCategory(FurnitureCategoryVM categoryVM)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                await _categoryService.AddCategory(categoryVM);
                return Ok(new ApiResponse { Message = "Tạo thành công", Status = "Success" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Lỗi : {ex.Message}");
            }
        }

        [Authorize(Roles = UserRoles.Admin)]
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteCategory(Guid id)
        {
            try
            {
                await _categoryService.DeleteCategory(id);
                return Ok(new ApiResponse { Message = "Xóa thành công", Status = "Success" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Lỗi : {ex.Message}");
            }
        }

        [Authorize(Roles = UserRoles.Admin)]
        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateCategory(Guid id, FurnitureCategoryVM categoryVM)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                await _categoryService.UpdateCategory(id, categoryVM);
                return Ok(new ApiResponse { Message = "Cập nhật thành công", Status = "Success" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Lỗi : {ex.Message}");
            }
        }
    }
}
