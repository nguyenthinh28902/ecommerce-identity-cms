using EcommerceIdentityCMS.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcommerceIdentityCMS.Core.Entities
{
    // Workplace.cs
    public class Workplace
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }

        // Đánh dấu Tổng công ty (Có quyền xem dữ liệu toàn hệ thống)
        public bool IsHeadquarters { get; set; }

        // Phân loại: "Kho", "Cửa hàng", "Văn phòng"
        public WorkplaceType Type { get; set; }

        // Quan hệ n:n với Phòng ban (Một địa điểm có nhiều phòng ban)
        public virtual ICollection<WorkplaceDepartment> WorkplaceDepartments { get; set; }
    }
}
