using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcommerceIdentityCMS.Core.Entities
{
    public class WorkplaceDepartment
    {
        public int WorkplaceId { get; set; }
        public virtual Workplace Workplace { get; set; }

        public int DepartmentId { get; set; }
        public virtual ApplicationDepartment Department { get; set; }
    }
}
