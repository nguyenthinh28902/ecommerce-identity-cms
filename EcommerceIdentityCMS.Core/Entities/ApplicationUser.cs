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
        public string Avatar {get; set; } = string.Empty;
        // 1:1 - Mỗi nhân viên thuộc biên chế tại một địa điểm duy nhất
        public int WorkplaceId { get; set; }
        // Thêm cột trạng thái hoạt động
        public bool IsActive { get; set; } = true;
        public bool IsDepartmentHead { get; set; } = false; // Quyền Trưởng phòng (dùng cho Tổng công ty)
        public virtual Workplace Workplace { get; set; }
        public ICollection<UserDepartment> UserDepartments { get; set; }
       = new List<UserDepartment>();

    }
}
