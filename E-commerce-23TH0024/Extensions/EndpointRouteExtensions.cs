namespace E_commerce_23TH0024.Extensions
{
    public static class EndpointRouteExtensions
    {
        public static void MapCustomRoutes(this IEndpointRouteBuilder endpoints)
        {
            endpoints.MapControllerRoute(
                name: "sanpham",
                pattern: "{tenLoaiSanPham}/{tenSanPham}-{id}",
                defaults: new { controller = "SanPhams_23TH0024", action = "Details" }
            );

            endpoints.MapControllerRoute(
                name: "areas",
                pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}"
            );
            endpoints.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}")
                .WithStaticAssets();

            endpoints.MapRazorPages()
               .WithStaticAssets();
        }
    }
}
