using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Nauktion.Areas.Identity.Data;
using Nauktion.Models;

[assembly: HostingStartup(typeof(Nauktion.Areas.Identity.IdentityHostingStartup))]
namespace Nauktion.Areas.Identity
{
    public class IdentityHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices((context, services) => {
                services.AddDbContext<NauktionContext>(options =>
                    options.UseSqlServer(
                        context.Configuration.GetConnectionString("NauktionContextConnection")));
                
                services.AddDefaultIdentity<NauktionUser>(config => config.Password = new PasswordOptions
                    {
                        RequireDigit = false,
                        RequireLowercase = false,
                        RequireNonAlphanumeric = false,
                        RequireUppercase = false,
                        RequiredLength = 7,
                        RequiredUniqueChars = 1
                    })
                    .AddRoles<NauktionRole>()
                    .AddEntityFrameworkStores<NauktionContext>();
            });
        }
    }
}