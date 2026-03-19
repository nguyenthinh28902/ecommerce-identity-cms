using EcommerceIdentityCMS.Api.Common.Requirement;
using EcommerceIdentityCMS.Core.Enums;
using EcommerceIdentityCMS.Core.Models.Settings;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;

namespace EcommerceIdentityCMS.Api.Common.Helpers
{
    public static class AuthenticationExtensions
    {
        public static IServiceCollection AddAuthenticationExtensions(this IServiceCollection services, IConfiguration configuration)
        {
            var _internalAuth = configuration
           .GetSection("InternalAuth")
           .Get<InternalAuth>()
           ?? throw new InvalidOperationException("JwtSettings missing");
            services.AddAuthentication("Bearer")
               .AddJwtBearer("Bearer", options =>
               {
                   // URL của IdentityServer - Dùng để tự động tải Metadata và Public Key
                   options.Authority = _internalAuth.Issuer;

                   // Chỉ để false khi đang ở môi trường Dev/Local không có SSL thật
                   options.RequireHttpsMetadata = false;

                   options.TokenValidationParameters = new TokenValidationParameters {
                       // Kiểm tra tính hợp lệ của Issuer (Người cấp phát)
                       ValidateIssuer = true,
                       ValidIssuer = _internalAuth.Issuer,

                       // Kiểm tra Audience (Mã định danh của API này)
                       ValidateAudience = true,
                       ValidAudience = _internalAuth.Audience,

                       // Kiểm tra thời hạn Token
                       ValidateLifetime = false,
                       // Độ trễ cho phép khi kiểm tra thời gian (Khuyên dùng 5-30s)
                       ClockSkew = TimeSpan.FromSeconds(20),

                       // BẮT BUỘC: Kiểm tra chữ ký của Token
                       ValidateIssuerSigningKey = true,
                       // Lưu ý: Vì đã có options.Authority ở trên, thư viện sẽ tự động 
                       // lấy Signing Key từ IdentityServer, ný không cần gán thủ công ở đây.

                   };
               });
            services.AddSingleton<IAuthorizationHandler, InternalOrPermissionHandler>();
            services.AddAuthorization(options =>
            {
                // Tất cả các Policy đều dùng chung Requirement, chỉ khác tham số Permission
                options.AddPolicy(PolicyNames.UserRead, policy =>
                    policy.AddRequirements(new InternalOrPermissionRequirement("user.read")));
                options.AddPolicy(PolicyNames.UserWrite, policy =>
                    policy.AddRequirements(new InternalOrPermissionRequirement("user.write")));
                // Nếu bạn vẫn muốn một Policy chỉ dành riêng cho internal (ví dụ các hàm admin hệ thống)
                options.AddPolicy(PolicyNames.Internal, policy =>
                {
                    policy.AddRequirements(new InternalOrPermissionRequirement("user.internal"));
                    policy.AddRequirements(new InternalOrPermissionRequirement("user.write"));
                    policy.AddRequirements(new InternalOrPermissionRequirement("user.read"));
                }

                 );
            });
            return services;
        }
    }
}
