using GMTWEB.Models;
using Microsoft.AspNetCore.Identity;

namespace GMTWEB.Data
{
    public static class DbInitializer
    {
        public static async Task Initialize(IApplicationBuilder app)
        {
            using (var serviceScope = app.ApplicationServices.CreateScope())
            {
                var userManager = serviceScope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();

                if (userManager.Users.Any())
                {
                    return;   
                }

                var adminUser = new ApplicationUser
                {
                    UserName = "admin@gmtweb.online",
                    Email = "admin@gmtweb.online",
                    EmailConfirmed = true 
                };

                var result = await userManager.CreateAsync(adminUser, "Admin@12345");

                if (!result.Succeeded)
                {
                    throw new InvalidOperationException("Could not create new user in seeder");
                }
            }
        }
    }
}