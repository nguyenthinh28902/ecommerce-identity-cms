using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcommerceIdentityCMS.Core.Entities
{
    // Bảng trung gian xử lý quyền CRUD theo chức năng
    public class DepartmentPermission
    {
        public int Id { get; set; }

        public int DepartmentId { get; set; }
        public virtual ApplicationDepartment Department { get; set; }

        // Mã chức năng: ví dụ "ORDER", "PRODUCT", "REPORT"
        public string FunctionCode { get; set; }

        // Quyền hạn CRUD
        public bool CanCreate { get; set; }
        public bool CanRead { get; set; }
        public bool CanUpdate { get; set; }
        public bool CanDelete { get; set; }
    }
}
