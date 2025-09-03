using Microsoft.AspNetCore.Identity;
using WeddingApp.Data;

namespace WeddingApp.Identity;

public static class IdentityExtensions
{
  
    
    public static async Task SeedIdentity(this WebApplication app)
    {
        using (var scope = app.Services.CreateScope())
        {
            var dbContext = scope.ServiceProvider.GetRequiredService<WeddingDbContext>();
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<WeddingAppUser>>();
            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            // Ensure database is created
            await dbContext.Database.EnsureCreatedAsync();

            // Seed roles
            var seedRoleNames = Enum.GetNames(typeof(Roles));
            
            foreach (var roleName in seedRoleNames)
            {
                if (!await roleManager.RoleExistsAsync(roleName))
                {
                    await roleManager.CreateAsync(new IdentityRole(roleName));
                    
                    if (roleName == nameof(Roles.Admin))
                    {
                        WeddingAppUser user;
                        // seed admin user
                        var adminEmail = app.Configuration.GetSection("SeedAdminEmail").Value;

                        if (!string.IsNullOrEmpty(adminEmail))
                        {
                            // create admin with no password
                            user = new WeddingAppUser()
                            {
                                UserName = adminEmail,
                                Email = adminEmail
                            };
                            
                            await userManager.CreateAsync(user);
                            await userManager.AddToRoleAsync(user, roleName);
                        }
                        
                    }
                }
            }
        }
    }

}