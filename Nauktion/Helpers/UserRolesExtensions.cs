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
            NauktionUser nauktionUser = await userManager.GetUserAsync(user);
            if (nauktionUser == null)
                return false;
            return await userManager.IsInRoleAsync(nauktionUser, role.ToString());
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

        public static async Task<IList<NauktionRoles>> GetNauktionRolesAsync(this UserManager<NauktionUser> userManager, ClaimsPrincipal user)
        {
            return (await userManager.GetRolesAsync(await userManager.GetUserAsync(user))).Select(role => Enum.Parse<NauktionRoles>(role, true)).ToList();
        }

        public static NauktionRoles HighestRole(this IEnumerable<NauktionRoles> roles)
        {
            return (NauktionRoles) roles.Cast<int>().Max();
        }

        public static string RoleLabelClassColor(this NauktionRoles role)
        {
            switch (role)
            {
                case NauktionRoles.Admin:
                    return "label-danger";

                default:
                    return "label-info";
            }
        }
    }
}