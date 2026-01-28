using EcommerceIdentityCMS.Core.Entities;
using EcommerceIdentityCMS.Core.Models.Settings;
using EcommerceIdentityCMS.Infrastructure.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcommerceIdentityCMS.Infrastructure.DependencyInjection
{
    public static class InfrastructureServiceRegistration
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
        {
            ConnectionStrings.EcommerceIdentityCMS = configuration.GetConnectionString("EcommerceIdentityCMS") ?? string.Empty;

            services.AddDbContext<EcommerceIdentityCMSContext>(options =>
               options.UseSqlServer(ConnectionStrings.EcommerceIdentityCMS, oracleOptions => { oracleOptions.CommandTimeout(60); }));
            // 1. Cấu hình IdentityCore cho ApplicationUser
            services.AddDataProtection();
            services.AddIdentityCore<ApplicationUser>(options => {
                options.Password.RequireDigit = false;
                options.Password.RequiredLength = 6;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireLowercase = false;
            })
            // 2. PHẢI CÓ: Đăng ký kiểu Role tùy chỉnh (ApplicationDepartment)
            .AddRoles<ApplicationDepartment>()
            // 3. PHẢI CÓ: Kết nối với DbContext
            .AddEntityFrameworkStores<EcommerceIdentityCMSContext>()
            // 4. Đăng ký các dịch vụ cần thiết cho Token (nếu dùng)
            .AddDefaultTokenProviders();

            return services;
        }
    }
}
