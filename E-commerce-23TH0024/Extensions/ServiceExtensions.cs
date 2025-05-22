using E_commerce_23TH0024.Models.Users;
using E_commerce_23TH0024.Service;

namespace E_commerce_23TH0024.Extensions
{
    public  static class ServiceExtensions
    {
        public static IServiceCollection AddAppServices(this IServiceCollection services)
        {
            services.AddScoped<SanPhamService>();
            services.AddScoped<DonHangService>();
            services.AddScoped<UserService>();
            return services;
        }
    }
}
