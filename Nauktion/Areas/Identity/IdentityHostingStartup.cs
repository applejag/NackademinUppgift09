using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
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
                
                services.AddIdentity<NauktionUser, NauktionRole>(config => config.Password = new PasswordOptions
                    {
                        RequireDigit = false,
                        RequireLowercase = false,
                        RequireNonAlphanumeric = false,
                        RequireUppercase = false,
                        RequiredLength = 7,
                        RequiredUniqueChars = 1
                    })
                    .AddDefaultUI()
                    .AddEntityFrameworkStores<NauktionContext>()
                    .AddDefaultTokenProviders();

                services.AddAuthentication()
                    .AddGoogle(googleOptions =>
                    {
                        googleOptions.ClientId = context.Configuration["Authentication:Google:ClientId"];
                        googleOptions.ClientSecret = context.Configuration["Authentication:Google:ClientSecret"];
                    });
            });
        }
    }
}