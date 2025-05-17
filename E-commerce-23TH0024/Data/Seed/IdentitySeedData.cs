using E_commerce_23TH0024.Models.Identity;
using Microsoft.AspNetCore.Identity;

namespace E_commerce_23TH0024.Data.Seed
{
    public class IdentitySeedData
    {
        public static async Task InitializeAsync(IServiceProvider serviceProvider)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();

            string[] roles = new[] { "Admin", "User" };

            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    await roleManager.CreateAsync(new IdentityRole(role));
                }
            }

            var adminEmail = "admin@gmail.com";
            var adminUser = await userManager.FindByEmailAsync(adminEmail);

            if (adminUser == null)
            {
                adminUser = new ApplicationUser
                {
                    UserName = adminEmail,
                    Email = adminEmail,
                    EmailConfirmed = true
                };
                var result = await userManager.CreateAsync(adminUser, "Admin123!");

                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(adminUser, "Admin");
                }
            }
            
            var normalUserEmail = "user@gmail.com";
            var normalUser = await userManager.FindByEmailAsync(normalUserEmail);
            if (normalUser == null)
            {
                normalUser = new ApplicationUser
                {
                    UserName = normalUserEmail,
                    Email = normalUserEmail,
                    EmailConfirmed = true
                };
                var result = await userManager.CreateAsync(normalUser, "User123!");

                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(normalUser, "User");
                }
            }
        }
    }
}
