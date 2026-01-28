using EcommerceIdentityCMS.Core.DTOs.Account;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcommerceIdentityCMS.Application.Sercivces.Interfaces
{
    public interface IAuthService
    {
        public Task<SignInResponseDto?> ValidateUserAsync(SignInRequestDto signInRequestDto);
    }
}
