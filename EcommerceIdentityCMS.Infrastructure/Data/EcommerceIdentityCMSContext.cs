using EcommerceIdentityCMS.Core.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcommerceIdentityCMS.Infrastructure.Data
{
    public class EcommerceIdentityCMSContext : IdentityDbContext<ApplicationUser, ApplicationDepartment, int>
    {
        public EcommerceIdentityCMSContext(DbContextOptions<EcommerceIdentityCMSContext> options) : base(options) { }

        // Các bảng nghiệp vụ bổ sung
        public DbSet<Workplace> Workplaces { get; set; }
        public DbSet<DepartmentPermission> DepartmentPermissions { get; set; }
        public DbSet<WorkplaceDepartment> WorkplaceDepartments { get; set; }
        public DbSet<UserDepartment> UserDepartments { get; set; }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // 1. Ánh xạ Identity sang tên nghiệp vụ (Phòng ban & Nhân sự)
            builder.Entity<ApplicationUser>(b => b.ToTable("Users"));
            builder.Entity<ApplicationDepartment>(b => b.ToTable("Departments")); // Role nay là Department

            // Đổi tên bảng trung gian Đa nhiệm (User - Role) thành UserDepartments
            builder.Entity<Microsoft.AspNetCore.Identity.IdentityUserRole<int>>(b => {
                b.ToTable("UserDepartments");
            });

            // 2. Cấu hình bảng trung gian Workplace - Department (n:n)
            // Một địa điểm có nhiều phòng ban và một loại phòng ban có ở nhiều nơi
            builder.Entity<WorkplaceDepartment>()
                .HasKey(wd => new { wd.WorkplaceId, wd.DepartmentId });

            builder.Entity<WorkplaceDepartment>()
                .HasOne(wd => wd.Workplace)
                .WithMany(w => w.WorkplaceDepartments)
                .HasForeignKey(wd => wd.WorkplaceId);

            builder.Entity<WorkplaceDepartment>()
                .HasOne(wd => wd.Department)
                .WithMany(d => d.WorkplaceDepartments)
                .HasForeignKey(wd => wd.DepartmentId);

            // 3. Cấu hình Nhân sự - Nơi làm việc (1:1 Biên chế)
            // Mỗi nhân viên chỉ thuộc biên chế tại một địa điểm duy nhất
            builder.Entity<ApplicationUser>()
                .HasOne(u => u.Workplace)
                .WithMany()
                .HasForeignKey(u => u.WorkplaceId)
                .OnDelete(DeleteBehavior.Restrict);

            // 4. Cấu hình Phân quyền CRUD (1:CRUD)
            // Phòng ban định nghĩa quyền hạn (Thêm, Xem, Sửa, Xóa)
            builder.Entity<DepartmentPermission>(b => {
                b.ToTable("DepartmentPermissions");
                b.HasOne(dp => dp.Department)
                 .WithMany(d => d.Permissions)
                 .HasForeignKey(dp => dp.DepartmentId);

             // Lưu Enum dưới dạng String trong DB (Tùy chọn)
             builder.Entity<Workplace>()
                    .Property(w => w.Type)
                    .HasConversion<string>();
            });
        }
    }
}
