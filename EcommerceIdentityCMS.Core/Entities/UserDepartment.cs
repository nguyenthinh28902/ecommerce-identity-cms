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
        // UserId và RoleId (DepartmentId) đã có sẵn từ IdentityUserRole

        public bool IsDepartmentHead { get; set; } // Quyền Trưởng phòng (dùng cho Tổng công ty)
        public bool IsPrimary { get; set; }        // Đánh dấu phòng ban chính

        public virtual ApplicationUser User { get; set; }
        public virtual ApplicationDepartment Department { get; set; }
    }
}
