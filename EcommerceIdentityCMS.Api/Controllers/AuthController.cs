using EcommerceIdentityCMS.Application.Sercivces.Interfaces;
using EcommerceIdentityCMS.Core.DTOs.Account;
using EcommerceIdentityCMS.Core.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EcommerceIdentityCMS.Api.Controllers
{
    [Route("api/xac-thuc")]
    [ApiController]
    [Authorize(PolicyNames.Internal)]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        public AuthController(IAuthService authService) => _authService = authService;

        [HttpPost("xac-thuc-noi-bo")]
        public async Task<IActionResult> AuthenticateInternal([FromBody] SignInRequestDto model)
        {
            var result = await _authService.ValidateUserAsync(model);
            if (result == null) return Unauthorized();
            return Ok(result); // Chỉ trả về Id và Email
        }

        [HttpGet("thong-tin-xac-thuc-nhan-su")]
        public async Task<IActionResult> GetAuthInfor()
        {
            var result = await _authService.GetSignInResponseDto();
            if (result == null) return Unauthorized();
            return Ok(result); // Chỉ trả về Id và Email
        }

    }
}
