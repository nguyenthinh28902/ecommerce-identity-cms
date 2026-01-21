using EcommerceIdentityCMS.Core.Models.Settings;
using EcommerceIdentityCMS.Infrastructure.Data;
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


            return services;
        }
    }
}
