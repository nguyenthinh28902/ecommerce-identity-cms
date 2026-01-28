using EcommerceIdentityCMS.Core.DTOs.ApplicationUser;
using EcommerceIdentityCMS.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcommerceIdentityCMS.Application.Sercivces.Interfaces
{
    public interface IUserSevice
    {
        public Task<Result<UserInforDto>> GetUserInfoAsync();
    }
}
