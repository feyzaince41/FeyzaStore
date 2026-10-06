using Microsoft.AspNetCore.Identity;

namespace FeyzaStore.Infrastructure.Identity;

public static class IdentitySeeder
{
    public static async Task SeedRolesAsync(
        RoleManager<IdentityRole> roleManager,
        UserManager<ApplicationUser> userManager)
    {
        // Admin rolü yoksa oluştur
        if (!await roleManager.RoleExistsAsync("Admin"))
        {
            await roleManager.CreateAsync(
                new IdentityRole("Admin")
            );
        }

        // Admin olacak kullanıcıyı bul
        var user = await userManager.FindByEmailAsync("feyza@test.com");

        // Kullanıcı varsa ve Admin değilse Admin rolünü ver
        if (user != null && !await userManager.IsInRoleAsync(user, "Admin"))
        {
            await userManager.AddToRoleAsync(user, "Admin");
        }
    }
}