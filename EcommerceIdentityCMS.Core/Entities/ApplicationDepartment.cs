using EcommerceIdentityCMS.Core.Enums;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcommerceIdentityCMS.Core.Entities
{
    public class ApplicationDepartment : IdentityRole<int>
    {
        public string Description { get; set; }

        // Thêm cột Code để dùng trong logic lập trình
        public DepartmentCode DeptCode { get; set; }

        // Quan hệ n:n với Nơi làm việc
        public virtual ICollection<WorkplaceDepartment> WorkplaceDepartments { get; set; }

        // Liên kết với bảng phân quyền CRUD
        public virtual ICollection<DepartmentPermission> Permissions { get; set; }

        public ICollection<UserDepartment> UserDepartments { get; set; }
    }
}
