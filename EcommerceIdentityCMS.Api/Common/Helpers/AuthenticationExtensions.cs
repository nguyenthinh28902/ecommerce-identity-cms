using EcommerceIdentityCMS.Api.Common.Requirement;
using EcommerceIdentityCMS.Core.Models.Settings;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;

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
                   options.Authority = _internalAuth.Issuer;
                   options.RequireHttpsMetadata = false;

                   options.TokenValidationParameters = new TokenValidationParameters {
                       ValidateIssuer = true,
                       ValidIssuer = _internalAuth.Issuer,

                       ValidateAudience = true,
                       ValidAudience = _internalAuth.Audience,

                       ValidateLifetime = true,
                       ClockSkew = TimeSpan.Zero,

                       ValidateIssuerSigningKey = true,

                       NameClaimType = JwtRegisteredClaimNames.Sub,
                       RoleClaimType = "role",
                   };
               });
            services.AddSingleton<IAuthorizationHandler, InternalOrPermissionHandler>();
            services.AddAuthorization(options =>
            {
                // Policy cho quyền Xem
                options.AddPolicy("user.read", policy =>
                    policy.AddRequirements(new InternalOrPermissionRequirement("user.read")));

                // Policy cho quyền Ghi
                options.AddPolicy("user.write", policy =>
                    policy.AddRequirements(new InternalOrPermissionRequirement("user.write")));
            });
            return services;
        }
    }
}
