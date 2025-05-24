using Microsoft.AspNetCore.DataProtection;

namespace E_commerce_23TH0024.Extensions
{
    public static class DataProtection
    {
        public static void ConfigureDataProtection(this IServiceCollection services, IWebHostEnvironment env)
        {
            var keysFolder = Path.Combine(env.ContentRootPath, "DataProtectionKeys");

            if (!Directory.Exists(keysFolder))
            {
                Directory.CreateDirectory(keysFolder);
            }

            services.AddDataProtection()
                .PersistKeysToFileSystem(new DirectoryInfo(keysFolder))
                .SetApplicationName("Ecommerce23TH0024");
        }
    }
}
