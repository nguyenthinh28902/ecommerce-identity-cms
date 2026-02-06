using EcommerceIdentityCMS.Core.Security.Models;

namespace EcommerceIdentityCMS.Api.Common.Helpers
{
    public static class ServiceDI
    {
        public static IServiceCollection AddServiceDI(this IServiceCollection services, IConfiguration configuration)
        {
            // Bind cấu hình vào Model
            services.Configure<InternalAuthHeader>(configuration.GetSection("InternalAuthHeader"));

            // Lấy giá trị ra để cấu hình HttpClient

            return services;
        }
    }
}
