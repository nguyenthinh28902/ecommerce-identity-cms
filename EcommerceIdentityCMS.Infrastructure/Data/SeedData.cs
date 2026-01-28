using EcommerceIdentityCMS.Core.Entities;
using EcommerceIdentityCMS.Core.Enums;
using EcommerceIdentityCMS.Infrastructure.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace EcommerceIdentityCMS.Infrastructure.Data
{
    public static class SeedData
    {
        public static async Task Initialize(IServiceProvider serviceProvider)
        {
            var context = serviceProvider.GetRequiredService<EcommerceIdentityCMSContext>();
            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            var roleManager = serviceProvider.GetRequiredService<RoleManager<ApplicationDepartment>>();

            const string testEmail = "nguyenngocthinhtest@gmail.com";

            // --- 1. KHỞI TẠO NƠI LÀM VIỆC ---
            if (!context.Workplaces.Any())
            {
                context.Workplaces.AddRange(
                    new Workplace { Name = "Tổng Công Ty Ecommerce Gemini GPT", IsHeadquarters = true, Type = WorkplaceType.Office, Address = "Số 1, Đường Gemini, TP. HCM" },
                    new Workplace { Name = "Gemini Store - Chi nhánh Quận 1", IsHeadquarters = false, Type = WorkplaceType.Store, Address = "123 Lê Lợi, Quận 1, TP. HCM" },
                    new Workplace { Name = "Kho Trung Tâm Gemini", IsHeadquarters = false, Type = WorkplaceType.Warehouse, Address = "Quận 1, TP. HCM" }
                );
                await context.SaveChangesAsync();
            }

            var tct = await context.Workplaces.FirstAsync(w => w.IsHeadquarters);
            var store = await context.Workplaces.FirstAsync(w => w.Type == WorkplaceType.Store);
            var wh = await context.Workplaces.FirstAsync(w => w.Type == WorkplaceType.Warehouse);

            // --- 2. KHỞI TẠO PHÒNG BAN (DEPARTMENTS) ---
            var departmentsToSeed = new List<(string Name, DepartmentCode Code)>
            {
                ("Admin", DepartmentCode.Admin),
                ("Quản lý", DepartmentCode.Manager),
                ("Kinh doanh", DepartmentCode.Business),
                ("Kế toán", DepartmentCode.Accountant),
                ("Quản trị Kho bãi", DepartmentCode.Warehouse),
                ("Điều vận & Cung ứng", DepartmentCode.Logistics),
                // PHÒNG BAN MỚI: CHUYÊN TRÁCH THƯƠNG MẠI ĐIỆN TỬ
                ("Nội dung Sản phẩm", DepartmentCode.Content)
            };

            foreach (var dept in departmentsToSeed)
            {
                if (!await roleManager.RoleExistsAsync(dept.Name))
                {
                    await roleManager.CreateAsync(new ApplicationDepartment
                    {
                        Name = dept.Name,
                        DeptCode = dept.Code,
                        Description = $"Phòng {dept.Name} hệ thống"
                    });
                }
            }

            // --- 3. KHỞI TẠO NHÂN SỰ (USERS) ---

            // A. TỔNG CÔNG TY
            await CreateUser(userManager, "admin_sys", testEmail, "Hệ Thống Admin", tct.Id, "Admin");
            await CreateUser(userManager, "mng_tct", testEmail, "Quản Lý Tổng Công Ty", tct.Id, "Quản lý");
            await CreateUser(userManager, "acc_tct", testEmail, "Kế Toán Tổng", tct.Id, "Kế toán");

            // USER MỚI: CHUYÊN VIÊN NỘI DUNG TẠI TỔNG CÔNG TY
            await CreateUser(userManager, "content_specialist", testEmail, "Chuyên viên Nội dung Sản phẩm", tct.Id, "Nội dung Sản phẩm");

            // Trưởng phòng kế toán tại TCT
            var headAcc = await CreateUser(userManager, "head_acc", testEmail, "Trưởng Phòng Kế Toán", tct.Id, "Kế toán", true);

            // B. CỬA HÀNG (STORE)
            await CreateUser(userManager, "mng_store_q1", testEmail, "Quản Lý Cửa Hàng Q1", store.Id, "Quản lý");
            await CreateUser(userManager, "sales_q1", testEmail, "Kinh Doanh Cửa Hàng Q1", store.Id, "Kinh doanh");
            await CreateUser(userManager, "acc_q1", testEmail, "Kế Toán Cửa Hàng Q1", store.Id, "Kế toán");
            await CreateUser(userManager, "inv_q1", testEmail, "Kho Cửa Hàng Q1", store.Id, "Quản trị Kho bãi");

            // C. KHO (WAREHOUSE)
            await CreateUser(userManager, "mng_wh", testEmail, "Quản Lý Kho Tổng", wh.Id, "Quản lý");
            await CreateUser(userManager, "staff_wh", testEmail, "Nhân Viên Kho Tổng", wh.Id, "Quản trị Kho bãi");
        }

        private static async Task<ApplicationUser> CreateUser(UserManager<ApplicationUser> um, string userName, string email, string fullName, int wpId, string roleName, bool IsDepartmentHead = false) 
        {
            var user = await um.FindByNameAsync(userName);
            if (user == null)
            {
                // Mặc định IsActive = true và xác nhận Email để có thể test ngay
                user = new ApplicationUser
                {
                    UserName = userName,
                    Email = email,
                    FullName = fullName,
                    WorkplaceId = wpId,
                    EmailConfirmed = true,
                    IsActive = true,
                    Avatar = "https://drive.google.com/file/d/1DQEDzJhz8wA1cQT2vSXTTpUNfeExkD4I/view?usp=drive_link"
                };

                var result = await um.CreateAsync(user, "Gemini@123");
                if (result.Succeeded)
                {
                    await um.AddToRoleAsync(user, roleName);
                }
            }
            return user;
        }

     
    }
}