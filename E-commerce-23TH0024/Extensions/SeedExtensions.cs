using E_commerce_23TH0024.Data.Seed;

namespace E_commerce_23TH0024.Extensions
{
    public static class SeedExtensions
    {
        public static async Task<IApplicationBuilder> UseIdentitySeedDataAsync(this IApplicationBuilder app)
        {
            using var scope = app.ApplicationServices.CreateScope();
            var services = scope.ServiceProvider;

            try
            {
                await IdentitySeedData.InitializeAsync(services);
            }
            catch (Exception ex)
            {
                var logger = services.GetRequiredService<ILoggerFactory>().CreateLogger("SeedExtensions");
                logger.LogError(ex, "Lỗi khi seed dữ liệu Identity");
            }

            return app;
        }
    }
}
