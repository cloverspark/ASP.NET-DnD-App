using Microsoft.AspNetCore.Identity;

namespace PC2.Data
{
    public static class IdentityHelper
    {
        public const string Admin = "Admin";

        internal static async Task CreateRoles(IServiceProvider provider, params string[] roles)
        {
            var roleManager = provider.GetRequiredService<RoleManager<IdentityRole>>();

            IdentityResult roleResult;

            foreach(string role in roles) 
            {
                bool doesRoleExist = await roleManager.RoleExistsAsync(role);
                if (!doesRoleExist)
                {
                    roleResult = await roleManager.CreateAsync(new IdentityRole(role));
                }
            }
        }
    }
}