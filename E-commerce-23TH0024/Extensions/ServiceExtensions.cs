using E_commerce_23TH0024.Models.Users;
using E_commerce_23TH0024.Service;
using E_commerce_23TH0024.Service.Api;
using Microsoft.Build.Evaluation;
using WorkManagement.Services;

namespace E_commerce_23TH0024.Extensions
{
    public  static class ServiceExtensions
    {
        public static IServiceCollection AddAppServices(this IServiceCollection services)
        {
            services.AddScoped<SanPhamService>();
            services.AddScoped<DonHangService>();
            services.AddScoped<UserService>();
            services.AddScoped<MenuService>();
            services.AddScoped<MoMoPaymentService>();
            services.AddScoped<VietQRPaymentService>();
            services.AddScoped<ProjectService>();
            return services;
        }
    }
}
