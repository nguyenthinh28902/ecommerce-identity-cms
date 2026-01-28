using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcommerceIdentityCMS.Core.DTOs.Account
{
    public class SignInRequestDto
    {
        public int Id { get; set; }
        public string Password { get; set; }
    }
}
