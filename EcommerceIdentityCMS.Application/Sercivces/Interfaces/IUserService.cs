using EcommerceIdentityCMS.Core.DTOs.ApplicationUser;
using EcommerceIdentityCMS.Core.Models;

namespace EcommerceIdentityCMS.Application.Sercivces.Interfaces
{
    public interface IUserService
    {
        public Task<Result<UserInforDto>> GetUserInfoAsync();
    }
}
