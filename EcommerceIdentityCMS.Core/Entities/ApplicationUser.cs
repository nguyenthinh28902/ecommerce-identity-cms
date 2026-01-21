using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcommerceIdentityCMS.Core.Entities
{
    public class ApplicationUser : IdentityUser<int>
    {
        public string FullName { get; set; }

        // 1:1 - Mỗi nhân viên thuộc biên chế tại một địa điểm duy nhất
        public int WorkplaceId { get; set; }
        public virtual Workplace Workplace { get; set; }

        // Logic Đa nhiệm: Identity mặc định hỗ trợ n:n giữa User và Role (Department)
        // Chúng ta sẽ dùng bảng mặc định AspNetUserRoles nhưng hiểu là UserDepartments
    }
}
