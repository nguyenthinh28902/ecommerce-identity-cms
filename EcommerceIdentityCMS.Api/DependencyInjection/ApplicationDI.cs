using EcommerceIdentityCMS.Application.DependencyInjection;

namespace EcommerceIdentityCMS.Api.DependencyInjection
{
    public static class ApplicationDI
    {
        public static IServiceCollection AddApplicationDI(
            this IServiceCollection services,
            IConfiguration configuration)
        {

            services.AddApplicationServices(configuration);

            return services;
        }
    }
}
