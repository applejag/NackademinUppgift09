using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Nauktion.Models;
using Nauktion.Repositories;
using Nauktion.Services;

namespace Nauktion
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            services.AddScoped<IAuktionRepository, AuktionRepository>();
            services.AddScoped<IAuktionService, AuktionService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(
            IApplicationBuilder app, 
            IHostingEnvironment env, 
            RoleManager<NauktionRole> roleManager,
            UserManager<NauktionUser> userManager)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseRequestLocalization(locOptions =>
            {
                var cultureInfoSvSe = new CultureInfo("sv-SE");
                locOptions.DefaultRequestCulture = new RequestCulture(cultureInfoSvSe);
                locOptions.SupportedCultures =
                locOptions.SupportedUICultures = new[]
                {
                    new CultureInfo("en-US"),
                    new CultureInfo("en-AU"),
                    new CultureInfo("en-GB"),
                    new CultureInfo("en"),
                    new CultureInfo("es-ES"),
                    new CultureInfo("es-MX"),
                    new CultureInfo("es"),
                    new CultureInfo("fr-FR"),
                    new CultureInfo("fr"),
                    cultureInfoSvSe,
                    new CultureInfo("sv"),
                };
            });
            app.UseAuthentication();
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();

            NauktionContext.SeedData(roleManager, userManager).GetAwaiter().GetResult();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
