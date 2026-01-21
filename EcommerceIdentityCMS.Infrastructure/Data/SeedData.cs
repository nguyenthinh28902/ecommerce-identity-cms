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
                    new Workplace { Name = "Tổng Công Ty Ecommerce Gemini GPT", IsHeadquarters = true, Type = WorkplaceType.Office }, //
                    new Workplace { Name = "Gemini Store - Chi nhánh Quận 1", IsHeadquarters = false, Type = WorkplaceType.Store }, //
                    new Workplace { Name = "Kho Trung Tâm Gemini", IsHeadquarters = false, Type = WorkplaceType.Warehouse } //
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
                ("Điều vận & Cung ứng", DepartmentCode.Logistics)
            };

            foreach (var dept in departmentsToSeed)
            {
                if (!await roleManager.RoleExistsAsync(dept.Name))
                {
                    await roleManager.CreateAsync(new ApplicationDepartment
                    {
                        Name = dept.Name,
                        DeptCode = dept.Code, //
                        Description = $"Phòng {dept.Name} hệ thống"
                    });
                }
            }

            // --- 3. KHỞI TẠO NHÂN SỰ (USERS) ---

            // A. TỔNG CÔNG TY
            await CreateUser(userManager, "admin_sys", testEmail, "Hệ Thống Admin", tct.Id, "Admin"); //
            await CreateUser(userManager, "mng_tct", testEmail, "Quản Lý Tổng Công Ty", tct.Id, "Quản lý"); //
            await CreateUser(userManager, "acc_tct", testEmail, "Kế Toán Tổng", tct.Id, "Kế toán"); //

            // Trưởng phòng kế toán tại TCT
            var headAcc = await CreateUser(userManager, "head_acc", testEmail, "Trưởng Phòng Kế Toán", tct.Id, "Kế toán"); //
            await SetDepartmentHead(context, headAcc.Id, DepartmentCode.Accountant, true); //

            // B. CỬA HÀNG (STORE)
            await CreateUser(userManager, "mng_store_q1", testEmail, "Quản Lý Cửa Hàng Q1", store.Id, "Quản lý"); //
            await CreateUser(userManager, "sales_q1", testEmail, "Kinh Doanh Cửa Hàng Q1", store.Id, "Kinh doanh"); //
            await CreateUser(userManager, "acc_q1", testEmail, "Kế Toán Cửa Hàng Q1", store.Id, "Kế toán"); //
            await CreateUser(userManager, "inv_q1", testEmail, "Kho Cửa Hàng Q1", store.Id, "Quản trị Kho bãi"); //

            // C. KHO (WAREHOUSE)
            await CreateUser(userManager, "mng_wh", testEmail, "Quản Lý Kho Tổng", wh.Id, "Quản lý"); //
            await CreateUser(userManager, "staff_wh", testEmail, "Nhân Viên Kho Tổng", wh.Id, "Quản trị Kho bãi"); //
        }

        private static async Task<ApplicationUser> CreateUser(UserManager<ApplicationUser> um, string userName, string email, string fullName, int wpId, string roleName)
        {
            var user = await um.FindByNameAsync(userName);
            if (user == null)
            {
                user = new ApplicationUser { UserName = userName, Email = email, FullName = fullName, WorkplaceId = wpId, EmailConfirmed = true };
                var result = await um.CreateAsync(user, "Gemini@123");
                if (result.Succeeded)
                {
                    await um.AddToRoleAsync(user, roleName); //
                }
            }
            return user;
        }

        private static async Task SetDepartmentHead(EcommerceIdentityCMSContext context, int userId, DepartmentCode deptCode, bool isHead)
        {
            var dept = await context.Roles.FirstOrDefaultAsync(r => r.DeptCode == deptCode); //
            if (dept != null)
            {
                // Truy vấn qua DbSet UserDepartments đã thêm vào Context
                var userDept = await context.UserDepartments.FirstOrDefaultAsync(ud => ud.UserId == userId && ud.RoleId == dept.Id); //
                if (userDept != null)
                {
                    userDept.IsDepartmentHead = isHead; //
                    context.UserDepartments.Update(userDept);
                    await context.SaveChangesAsync();
                }
            }
        }
    }
}