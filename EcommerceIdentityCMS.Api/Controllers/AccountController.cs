using EcommerceIdentityCMS.Application.Sercivces.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EcommerceIdentityCMS.Api.Controllers
{
    [Route("api/tai-khoan")]
    [ApiController]
    [Authorize(Policy = "user.read")]
    public class AccountController : ControllerBase
    {
        private readonly IUserSevice _userSevice;
        public AccountController(IUserSevice userSevice)
        {
            _userSevice = userSevice;
        }

        [HttpGet("thong-tin-nhan-su")]
        public async Task<IActionResult> GetInformation()
        {
            var infor = await _userSevice.GetUserInfoAsync();
            return Ok(infor);
        }
    }
}
