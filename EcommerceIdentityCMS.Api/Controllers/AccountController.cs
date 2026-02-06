using EcommerceIdentityCMS.Application.Sercivces.Interfaces;
using EcommerceIdentityCMS.Core.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EcommerceIdentityCMS.Api.Controllers
{
    [Route("api/tai-khoan")]
    [ApiController]
    [Authorize(PolicyNames.UserRead)]
    public class AccountController : ControllerBase
    {
        private readonly IUserService _userSevice;
        public AccountController(IUserService userSevice)
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
