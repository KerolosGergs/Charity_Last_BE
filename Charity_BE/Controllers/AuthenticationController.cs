using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.DTOS.AuthDTO;
using Shared.DTOS.Common;
using BLL.ServiceAbstraction;
using System.Net;

namespace Charity_BE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly ILogger<AuthenticationController> _logger;

        public AuthenticationController(IAuthService authService, ILogger<AuthenticationController> logger)
        {
            _authService = authService;
            _logger = logger;
        }

        // POST: api/authentication/login
        [HttpPost("login")]
        public async Task<ActionResult<ApiResponse<AuthResponseDTO>>> Login([FromBody] LoginDTO loginDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var errors = ModelState.Values
                        .SelectMany(v => v.Errors)
                        .Select(e => e.ErrorMessage)
                        .ToList();
                    return BadRequest(ApiResponse<AuthResponseDTO>.ErrorResult("بيانات الإدخال غير صالحة", 400, errors));
                }

                var result = await _authService.LoginAsync(loginDto);
                return Ok(ApiResponse<AuthResponseDTO>.SuccessResult(result, result.Message));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "خطأ في تسجيل الدخول للمستخدم: {Email}", loginDto.Email);

                var errorMessage = !string.IsNullOrEmpty(ex.Message) ? ex.Message : "حدث خطأ في تسجيل الدخول";
                return BadRequest(ApiResponse<AuthResponseDTO>.ErrorResult(errorMessage, 400));
            }
        }

        // POST: api/authentication/register
        [HttpPost("register")]
        public async Task<ActionResult<ApiResponse<AuthResponseDTO>>> Register([FromBody] RegisterDTO registerDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var errors = ModelState.Values
                        .SelectMany(v => v.Errors)
                        .Select(e => e.ErrorMessage)
                        .ToList();
                    return BadRequest(ApiResponse<AuthResponseDTO>.ErrorResult("بيانات الإدخال غير صالحة", 400, errors));
                }

                var result = await _authService.RegisterAsync(registerDto);
                return Ok(ApiResponse<AuthResponseDTO>.SuccessResult(result, result.Message));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "خطأ في تسجيل المستخدم: {Email}", registerDto.Email);

                var errorMessage = !string.IsNullOrEmpty(ex.Message) ? ex.Message : "حدث خطأ في إنشاء الحساب";
                return BadRequest(ApiResponse<AuthResponseDTO>.ErrorResult(errorMessage, 400));
            }
        }

        // POST: api/authentication/register-admin
        [HttpPost("register-admin")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<ApiResponse<AuthResponseDTO>>> RegisterAdmin([FromBody] RegisterAdminDTO registerDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var errors = ModelState.Values
                        .SelectMany(v => v.Errors)
                        .Select(e => e.ErrorMessage)
                        .ToList();
                    return BadRequest(ApiResponse<AuthResponseDTO>.ErrorResult("بيانات الإدخال غير صالحة", 400, errors));
                }

                var result = await _authService.RegisterAdminAsync(registerDto);
                return Ok(ApiResponse<AuthResponseDTO>.SuccessResult(result, result.Message));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "خطأ في تسجيل المدير: {Email}", registerDto.Email);

                var errorMessage = !string.IsNullOrEmpty(ex.Message) ? ex.Message : "حدث خطأ في إنشاء حساب المدير";
                return BadRequest(ApiResponse<AuthResponseDTO>.ErrorResult(errorMessage, 400));
            }
        }

        // POST: api/authentication/register-advisor
        [HttpPost("register-advisor")]
        //[Authorize(Roles = "Admin")]
        public async Task<ActionResult<ApiResponse<AuthResponseDTO>>> RegisterAdvisor([FromBody] RegisterAdvisorDTO registerDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var errors = ModelState.Values
                        .SelectMany(v => v.Errors)
                        .Select(e => e.ErrorMessage)
                        .ToList();
                    return BadRequest(ApiResponse<AuthResponseDTO>.ErrorResult("بيانات الإدخال غير صالحة", 400, errors));
                }

                var result = await _authService.RegisterAdvisorAsync(registerDto);
                return Ok(ApiResponse<AuthResponseDTO>.SuccessResult(result, result.Message));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "خطأ في تسجيل المستشار: {Email}", registerDto.Email);

                var errorMessage = !string.IsNullOrEmpty(ex.Message) ? ex.Message : "حدث خطأ في إنشاء حساب المستشار";
                return Ok(ApiResponse<AuthResponseDTO>.ErrorResult(errorMessage, 200));
            }
        }

        // GET: api/authentication/me
        [HttpGet("me")]
        [Authorize]
        public async Task<ActionResult<ApiResponse<CurrentUserDTO>>> GetCurrentUser()
        {
            try
            {
                var userEmail = User.FindFirst(System.Security.Claims.ClaimTypes.Email)?.Value;
                if (string.IsNullOrEmpty(userEmail))
                {
                    return Unauthorized(ApiResponse<CurrentUserDTO>.ErrorResult("المستخدم غير مصرح له بالدخول", 401));
                }

                var user = await _authService.GetCurrentUserAsync(userEmail);
                if (user == null)
                {
                    return NotFound(ApiResponse<CurrentUserDTO>.ErrorResult("المستخدم غير موجود", 404));
                }

                return Ok(ApiResponse<CurrentUserDTO>.SuccessResult(user));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "خطأ في الحصول على بيانات المستخدم الحالي");
                return StatusCode(500, ApiResponse<CurrentUserDTO>.ErrorResult("حدث خطأ في الحصول على بيانات المستخدم", 500));
            }
        }

        // POST: api/authentication/forgot-password
        [HttpPost("forgot-password")]
        public async Task<ActionResult<ApiResponse<string>>> ForgotPassword([FromBody] forgotPasswordDTO dto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var errors = ModelState.Values
                        .SelectMany(v => v.Errors)
                        .Select(e => e.ErrorMessage)
                        .ToList();
                    return BadRequest(ApiResponse<string>.ErrorResult("بيانات الإدخال غير صالحة", 400, errors));
                }

                await _authService.ForgotPasswordAsync(dto.Email);
                return Ok(ApiResponse<string>.SuccessResult("إذا كان البريد الإلكتروني مسجل لدينا، ستتلقى رابط إعادة تعيين كلمة المرور"));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "خطأ في طلب إعادة تعيين كلمة المرور: {Email}", dto.Email);

                var errorMessage = !string.IsNullOrEmpty(ex.Message) ? ex.Message : "حدث خطأ في معالجة طلب إعادة تعيين كلمة المرور";
                return StatusCode(500, ApiResponse<string>.ErrorResult(errorMessage, 500));
            }
        }

        // POST: api/authentication/reset-password?token=...
        [HttpPost("reset-password")]
        public async Task<ActionResult<ApiResponse<string>>> ResetPassword([FromBody] ResetPasswordDTO dto, [FromQuery] string token)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var errors = ModelState.Values
                        .SelectMany(v => v.Errors)
                        .Select(e => e.ErrorMessage)
                        .ToList();
                    return BadRequest(ApiResponse<string>.ErrorResult("بيانات الإدخال غير صالحة", 400, errors));
                }

                if (string.IsNullOrEmpty(token))
                {
                    return BadRequest(ApiResponse<string>.ErrorResult("رمز إعادة التعيين مطلوب", 400));
                }

                var result = await _authService.ResetPasswordAsync(dto, token);
                return Ok(ApiResponse<string>.SuccessResult("تم إعادة تعيين كلمة المرور بنجاح"));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "خطأ في إعادة تعيين كلمة المرور: {Email}", dto.Email);

                var errorMessage = !string.IsNullOrEmpty(ex.Message) ? ex.Message : "حدث خطأ في إعادة تعيين كلمة المرور";
                return BadRequest(ApiResponse<string>.ErrorResult(errorMessage, 400));
            }
        }

        // GET: api/authentication/check-email/{email}
        [HttpGet("check-email/{email}")]
        public async Task<ActionResult<ApiResponse<bool>>> CheckEmailExists(string email)
        {
            try
            {
                if (string.IsNullOrEmpty(email) || !email.Contains("@"))
                {
                    return BadRequest(ApiResponse<bool>.ErrorResult("البريد الإلكتروني غير صالح", 400));
                }

                var exists = await _authService.IsEmailExistAsync(email);
                return Ok(ApiResponse<bool>.SuccessResult(exists));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "خطأ في فحص البريد الإلكتروني: {Email}", email);
                return StatusCode(500, ApiResponse<bool>.ErrorResult("حدث خطأ في فحص البريد الإلكتروني", 500));
            }
        }
    }
}