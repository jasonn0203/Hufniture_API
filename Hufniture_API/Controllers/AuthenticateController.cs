using Hufniture_API.Data;
using Hufniture_API.Services.TokenService;
using Hufniture_API.ViewModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

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
                return Unauthorized(new ApiResponse { Status = "Error", Message = "Invalid credentials!" });
            }

            var token = await _tokenService.GenerateTokenAsync(user);

            return Ok(new { Token = token, User = user.FullName });
        }


        [HttpPost]
        [Route("forgot-password")]
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



       

    }
}
