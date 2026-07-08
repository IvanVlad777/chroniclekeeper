using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;

namespace ChronicleKeeper.Infrastructure.Data
{
    public static class DbSeeder
    {
        public static async Task SeedRolesAndAdmin(IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.CreateScope();
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();
            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            string[] roleNames = { "SuperAdmin", "Admin", "Editor", "Moderator", "Reader" };

            // 🛠️ Ensure all roles exist
            foreach (var roleName in roleNames)
            {
                if (!await roleManager.RoleExistsAsync(roleName))
                {
                    await roleManager.CreateAsync(new IdentityRole(roleName));
                }
            }

            // 🛠️ Ensure SuperAdmin User Exists
            await CreateUserIfNotExists(userManager, "superadmin@chroniclekeeper.com", "SuperAdmin@123", "SuperAdmin");

            // 🛠️ Ensure Admin User Exists
            await CreateUserIfNotExists(userManager, "admin@chroniclekeeper.com", "Admin@123", "Admin");
        }

        public static async Task SeedDemoWorld(IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();

            if (await context.Worlds.AnyAsync()) return;

            var superAdmin = await userManager.FindByEmailAsync("superadmin@chroniclekeeper.com");
            if (superAdmin == null) return;

            context.Worlds.Add(new Core.Entities.Worlds.World
            {
                Name = "Demo World",
                Description = "Automatski stvoren svijet za isprobavanje aplikacije.",
                OwnerId = superAdmin.Id
            });
            await context.SaveChangesAsync();
        }

        private static async Task CreateUserIfNotExists(UserManager<IdentityUser> userManager, string email, string password, string role)
        {
            if (await userManager.FindByEmailAsync(email) == null)
            {
                var user = new IdentityUser
                {
                    UserName = email,
                    Email = email,
                    EmailConfirmed = true
                };

                var result = await userManager.CreateAsync(user, password);
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(user, role);
                }
            }
        }
    }
}
