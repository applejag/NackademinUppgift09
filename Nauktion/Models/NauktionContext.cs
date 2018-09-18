using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Nauktion.Helpers;

namespace Nauktion.Models
{
    public class NauktionContext : IdentityDbContext<NauktionUser, NauktionRole, string>
    {
        public NauktionContext(DbContextOptions<NauktionContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);
        }

        public static async Task SeedData(RoleManager<NauktionRole> roleManager, UserManager<NauktionUser> userManager)
        {

            // Seed roles
            foreach (string roleName in Enum.GetNames(typeof(NauktionRoles)))
            {
                if (!await roleManager.RoleExistsAsync(roleName))
                {
                    await roleManager.CreateAsync(new NauktionRole(roleName));
                }
            }

            // Everyone has role Regular
            foreach (NauktionUser user in userManager.Users.ToList())
            {
                if (!await userManager.IsInRoleAsync(user, NauktionRoles.Regular))
                {
                    await userManager.AddToRoleAsync(user, NauktionRoles.Regular);
                }
            }

        }
    }
}
