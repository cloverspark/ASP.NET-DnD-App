using Microsoft.AspNetCore.Identity;

namespace PC2.Data
{
    public static class IdentityHelper
    {
        public const string Admin = "Admin";
        public const string DungeonMaster = "DungeonMaster";
        public const string BasicPlayer = "BasicPlayer";

        internal static async Task CreateRoles(IServiceProvider provider, params string[] roles)
        {
            var roleManager = provider.GetRequiredService<RoleManager<IdentityRole>>();

            IdentityResult roleResult;

            foreach(string role in roles) // Go through all the user roles
            {
                bool doesRoleExist = await roleManager.RoleExistsAsync(role);
                if (!doesRoleExist) // If the user role does not exist create it
                {
                    roleResult = await roleManager.CreateAsync(new IdentityRole(role));
                }
            }
        }
    }
}