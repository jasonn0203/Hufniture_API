using Hufniture_API.Data;
using Hufniture_API.Services.TokenService;
using Hufniture_API.ViewModel;
using Hufniture_API.ViewModel.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Hufniture_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticateController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _configuration;
        private readonly ITokenService _tokenService;
        private readonly IPasswordHasher<AppUser> _passwordHasher;

        public AuthenticateController(UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager, IConfiguration configuration, ITokenService tokenService, IPasswordHasher<AppUser> passwordHasher)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _configuration = configuration;
            _tokenService = tokenService;
            _passwordHasher = passwordHasher;
        }

        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register([FromBody] RegisterVM model)
        {
            //Check validate
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors)
                                              .Select(e => e.ErrorMessage)
                                              .ToList();
                return BadRequest(new ApiResponse { Status = "Error", Message = string.Join(", ", errors) });
            }


            var userExists = await _userManager.FindByEmailAsync(model.Email);
            if (userExists != null)
            {
                return BadRequest(new ApiResponse { Status = "Error", Message = "Email đã tồn tại trong hệ thống!" });
            }

            var user = new AppUser
            {
                UserName = model.Email,
                FullName = model.Name,
                Email = model.Email,
                PhoneNumber = model.PhoneNumber

            };

            var result = await _userManager.CreateAsync(user, model.Password);
            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                return StatusCode(StatusCodes.Status500InternalServerError, new ApiResponse { Status = "Error", Message = $"Đăng ký người dùng thất bại! Errors: {errors}" });
            }

            // Kiểm tra và tạo vai trò nếu cần thiết
            if (!await _roleManager.RoleExistsAsync(UserRoles.User))
            {
                await _roleManager.CreateAsync(new IdentityRole(UserRoles.User));
            }

            await _userManager.AddToRoleAsync(user, UserRoles.User);

            return Ok(new ApiResponse { Status = "Success", Message = "Đăng ký người dùng thành công!" });
        }


        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] LoginVM model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);

            if (user == null || !await _userManager.CheckPasswordAsync(user, model.Password))
            {
                return Unauthorized(new ApiResponse { Status = "Error", Message = "Password hoặc Email không đúng!" });
            }

            var token = await _tokenService.GenerateTokenAsync(user);

            var userResponse = new UserResponseVM
            {
                Email = user.Email,
                Address = user.Address,
                FullName = user.FullName,
                BirthDate = user.BirthDate,
                Gender = user.Gender,
                Id = user.Id
            };

            return Ok(new { token = token, user = userResponse });
        }


        [HttpPost]
        [Route("CheckEmail")]
        public async Task<IActionResult> CheckEmail([FromBody] CheckEmailVM model)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors)
                                              .Select(e => e.ErrorMessage)
                                              .ToList();
                return BadRequest(new ApiResponse { Status = "Error", Message = string.Join(", ", errors) });
            }

            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                return Ok(new ApiResponse { Status = "Error", Message = "Email không tồn tại." });
            }

            return Ok(new ApiResponse { Status = "Success", Message = "Email tồn tại." });
        }



        [HttpPost]
        [Route("ForgotPassword")]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordVM model)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors)
                                              .Select(e => e.ErrorMessage)
                                              .ToList();
                return BadRequest(new ApiResponse { Status = "Error", Message = string.Join(", ", errors) });
            }

            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                return BadRequest(new ApiResponse { Status = "Error", Message = "Không tìm thấy người dùng với địa chỉ email này." });
            }

            // Generate hash for new password
            var newPasswordHash = _passwordHasher.HashPassword(user, model.NewPassword);

            // Update user's PasswordHash in database
            user.PasswordHash = newPasswordHash;
            var updateResult = await _userManager.UpdateAsync(user);

            if (!updateResult.Succeeded)
            {
                var errors = string.Join(", ", updateResult.Errors.Select(e => e.Description));
                return BadRequest(new ApiResponse { Status = "Error", Message = $"Cập nhật mật khẩu thất bại! Errors: {errors}" });
            }

            return Ok(new ApiResponse { Status = "Success", Message = "Đặt lại mật khẩu thành công!" });
        }

        [Authorize]
        [HttpPut]
        [Route("UpdateUser/{userId}")]
        public async Task<IActionResult> UpdateUser(string userId,[FromBody] UpdateUserInfoVM model)
        {
            // Lấy user hiện tại
            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
            {
                return NotFound(new ApiResponse { Status = "Error", Message = "Không tìm thấy người dùng." });
            }

            // Cập nhật các thông tin có trong model
            if (!string.IsNullOrEmpty(model.Address))
            {
                user.Address = model.Address;
            }

            if (!string.IsNullOrEmpty(model.FullName))
            {
                user.FullName = model.FullName;
            }

            if (model.BirthDate.HasValue)
            {
                user.BirthDate = model.BirthDate;
            }

            if (!string.IsNullOrEmpty(model.PhoneNumber))
            {
                user.PhoneNumber = model.PhoneNumber;
            }

            if (model.Gender.HasValue)
            {
                user.Gender = model.Gender;
            }

            // Cập nhật thông tin user trong database
            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                return StatusCode(StatusCodes.Status500InternalServerError, new ApiResponse { Status = "Error", Message = $"Cập nhật thông tin người dùng thất bại! Errors: {errors}" });
            }

            return Ok(new ApiResponse { Status = "Success", Message = "Cập nhật thông tin người dùng thành công!" });
        }


        [Authorize]
        [HttpGet]
        [Route("GetUserInfo/{userId}")]
        public async Task<IActionResult> GetUserInfo(string userId)
        {
            // Lấy user hiện tại
            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
            {
                return NotFound(new ApiResponse { Status = "Error", Message = "Không tìm thấy người dùng." });
            }

            var userResponse = new UserResponseVM
            {
                Id = user.Id,
                Email = user.Email!,
                Address = user.Address,
                FullName = user.FullName,
                BirthDate = user.BirthDate,
                Gender = user.Gender,
                PhoneNumber = user.PhoneNumber
            };

            return Ok(userResponse);
        }


        [Authorize]
        [HttpPost]
        [Route("ChangePassword/{userId}")]
        public async Task<IActionResult> ChangePassword(string userId, [FromBody] ChangePasswordVM model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Tìm user dựa trên userId
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return NotFound("Không tìm thấy user này");
            }

            // Kiểm tra mật khẩu hiện tại
            var passwordCheck = await _userManager.CheckPasswordAsync(user, model.CurrentPassword);
            if (!passwordCheck)
            {
                return BadRequest("Mật khẩu hiện tại không chính xác !");
            }

            // Kiểm tra mật khẩu mới và xác nhận mật khẩu mới
            if (model.NewPassword != model.ConfirmNewPassword)
            {
                return BadRequest("Mật khẩu mới không trùng với nhập lại mật khẩu mới");
            }

            // Thay đổi mật khẩu
            var result = await _userManager.ChangePasswordAsync(user, model.CurrentPassword, model.NewPassword);
            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }

            return Ok("Đổi mật khẩu thành công !");
        }


    }
}
