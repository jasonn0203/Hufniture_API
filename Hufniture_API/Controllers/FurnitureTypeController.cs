using Hufniture_API.Data;
using Hufniture_API.Models;
using Hufniture_API.Services.FurnitureTypeService;
using Hufniture_API.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hufniture_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FurnitureTypeController : ControllerBase
    {
        private readonly IFurnitureTypeService _typeService;

        public FurnitureTypeController(IFurnitureTypeService typeService)
        {
            _typeService = typeService;
        }

        [HttpGet("GetAllFurnitureTypes")]
        public async Task<ActionResult<IEnumerable<FurnitureType>>> GetFurnitureTypes()
        {
            try
            {
                var types = await _typeService.GetAll();
                if (types == null || !types.Any())
                {
                    return NotFound(new ApiResponse { Message = "Không có loại nào được tìm thấy.", Status = "Success" });
                }
                var response = new
                {
                    results = types
                };

                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Lỗi : {ex.Message}");
            }
        }



        [Authorize(Roles = UserRoles.Admin)]
        [HttpPost]
        public async Task<ActionResult> AddFurnitureType(FurnitureTypeVM typeVM)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                await _typeService.AddType(typeVM);
                return Ok(new ApiResponse { Message = "Tạo thành công", Status = "Success" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Lỗi : {ex.Message}");
            }
        }


        [Authorize(Roles = UserRoles.Admin)]
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteType(Guid id)
        {
            try
            {
                await _typeService.DeleteType(id);
                return Ok(new ApiResponse { Message = "Xóa thành công", Status = "Success" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Lỗi : {ex.Message}");
            }
        }

        [Authorize(Roles = UserRoles.Admin)]
        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateCategory(Guid id, FurnitureTypeVM typeVM)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                await _typeService.UpdateType(id, typeVM);
                return Ok(new ApiResponse { Message = "Cập nhật thành công", Status = "Success" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Lỗi : {ex.Message}");
            }
        }


    }
}
