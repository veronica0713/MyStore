using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Internal;
using MyStore.Models;

namespace MyStore
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
            //These are not part of .NET Core... they are seperate libraries that must be installed via a program called NuGet.
            //Right click on your Project -> Manage NuGet Packages

            Configuration.GetConnectionString("Boats");

            string boatChartersConnectionString = Configuration.GetConnectionString("Boats");

            //using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
            //using Microsoft.EntityFrameworkCore;
            //using Microsoft.AspNetCore.Identity;


            services.AddDbContext<BoatChartersDbContext>(opt => opt.UseSqlServer(boatChartersConnectionString));

            services.AddIdentity<BoatChartesUser, IdentityRole>(options =>
                {
                    options.Password.RequireDigit = false;
                    options.Password.RequireNonAlphanumeric = false;
                    options.Password.RequireLowercase = false;
                    options.Password.RequireUppercase = false;
                })
                .AddEntityFrameworkStores<BoatChartersDbContext>()
                .AddDefaultTokenProviders();

            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, BoatChartersDbContext db)
        {
            if (env.IsDevelopment())
            {
                app.UseBrowserLink();
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }
            //I need this to instruct my app to use Cookies for tracking SignIn/SignOut status
            app.UseAuthentication();
            app.UseStaticFiles();
            
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");

                DbInitializer.Initialize(db);
            });
           
        }
    }
}
