using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcommerceIdentityCMS.Core.Entities
{
    public class UserDepartment : IdentityUserRole<int>
    {
        public bool IsPrimary { get; set; } = true;
        public virtual ApplicationUser User { get; set; } = null!;
        public virtual ApplicationDepartment Department { get; set; } = null!;
    }

}
