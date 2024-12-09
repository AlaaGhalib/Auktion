using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace Auktion.Areas.Identity.Data
{
    public static class RoleInitializer
    {
        public static async Task InitializeRolesAndAdminAsync(IServiceProvider serviceProvider)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = serviceProvider.GetRequiredService<UserManager<AuktionUser>>();

            if (!await roleManager.RoleExistsAsync("Administrator"))
            {
                await roleManager.CreateAsync(new IdentityRole("Administrator"));
            }

            var adminEmail = "admin@example.com";
            var adminPassword = "Admin123";

            var adminUser = await userManager.FindByEmailAsync(adminEmail);
            if (adminUser == null)
            { 
                adminUser = new AuktionUser
                {
                    UserName = adminEmail,
                    Email = adminEmail,
                    EmailConfirmed = true
                };
                var createResult = await userManager.CreateAsync(adminUser, adminPassword);
                if (!createResult.Succeeded)
                {
                    foreach (var error in createResult.Errors)
                    {
                        Console.WriteLine($"Error creating admin user: {error.Description}");
                    }
                    return;
                }
            }
            if (!await userManager.IsInRoleAsync(adminUser, "Administrator"))
            {
                await userManager.AddToRoleAsync(adminUser, "Administrator");
            }
            var existingClaims = await userManager.GetClaimsAsync(adminUser);
            var adminClaim = existingClaims.FirstOrDefault(c => c.Type == "AdminClaim");

            if (adminClaim == null || adminClaim.Value != "True")
            {
                if (adminClaim != null)
                {
                    await userManager.RemoveClaimAsync(adminUser, adminClaim);
                }
                var claimResult = await userManager.AddClaimAsync(adminUser, new Claim("AdminClaim", "True"));
                if (!claimResult.Succeeded)
                {
                    foreach (var error in claimResult.Errors)
                    {
                        Console.WriteLine($"Error adding AdminClaim: {error.Description}");
                    }
                }
            }
        }
    }
}
