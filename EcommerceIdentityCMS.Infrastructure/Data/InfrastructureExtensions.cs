using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace EcommerceIdentityCMS.Infrastructure.Data
{
    public static class InfrastructureExtensions
    {
        public static async Task SeedDatabaseAsync(this IServiceProvider serviceProvider)
        {
            using (var scope = serviceProvider.CreateScope())
            {
                var services = scope.ServiceProvider;
                var loggerFactory = services.GetRequiredService<ILoggerFactory>();

                try
                {
                    // Gọi hàm Initialize từ file SeedData.cs của bạn
                    await SeedData.Initialize(services);
                }
                catch (Exception ex)
                {
                    var logger = loggerFactory.CreateLogger("InfrastructureExtensions");
                    logger.LogError(ex, "Lỗi xảy ra trong quá trình Seed Data hệ thống.");
                    throw; // Ném lỗi để biết nếu có vấn đề nghiêm trọng khi khởi động
                }
            }
        }
    }
}
