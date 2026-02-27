using EcommerceIdentityCMS.Application.DependencyInjection;
using EcommerceIdentityCMS.Infrastructure.DependencyInjection;

namespace EcommerceIdentityCMS.Api.DependencyInjection
{
    public static class ApplicationDI
    {
        public static IServiceCollection AddApplicationDI(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            // khai báo Infrastructure (EcomProductDbContext)
            services.AddInfrastructureServices(configuration);
            services.AddApplicationServices(configuration);

            return services;
        }
    }
}
