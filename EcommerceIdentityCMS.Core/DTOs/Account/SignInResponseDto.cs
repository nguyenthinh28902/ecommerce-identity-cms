using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcommerceIdentityCMS.Core.DTOs.Account
{
    public class SignInResponseDto
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public List<string> Roles { get; set; }
        public int WorkplaceId { get; set; }
        public List<string> Scopes { get; set; }
    }
}
