using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Nauktion.Models;

namespace Nauktion.Helpers
{
    public static class UserRolesExtensions
    {
        public static async Task<bool> IsInRoleAsync(this UserManager<NauktionUser> userManager, ClaimsPrincipal user, NauktionRoles role)
        {
            return await userManager.IsInRoleAsync(await userManager.GetUserAsync(user), role.ToString());
        }

        public static async Task<bool> IsInRoleAsync(this UserManager<NauktionUser> userManager, NauktionUser user, NauktionRoles role)
        {
            return await userManager.IsInRoleAsync(user, role.ToString());
        }

        public static async Task<IdentityResult> AddToRoleAsync(this UserManager<NauktionUser> userManager, NauktionUser user, NauktionRoles role)
        {
            return await userManager.AddToRoleAsync(user, role.ToString());
        }

        public static async Task<IdentityResult> AddToRolesAsync(this UserManager<NauktionUser> userManager, NauktionUser user, params NauktionRoles[] roles)
        {
            return await userManager.AddToRolesAsync(user, roles.Select(role => role.ToString()));
        }
        
        public static async Task<IdentityResult> AddToRolesAsync(this UserManager<NauktionUser> userManager, NauktionUser user, IEnumerable<NauktionRoles> roles)
        {
            return await userManager.AddToRolesAsync(user, roles.Select(role => role.ToString()));
        }

        public static async Task<IList<NauktionRoles>> GetNauktionRolesAsync(this UserManager<NauktionUser> userManager, NauktionUser user)
        {
            return (await userManager.GetRolesAsync(user)).Select(role => Enum.Parse<NauktionRoles>(role, true)).ToList();
        }
    }
}