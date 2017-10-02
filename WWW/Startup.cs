using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
//using AppConfigurationSettings;
using WWW.Models;
using Services;

namespace WWW
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
            services.AddDbContext<DataAccess.HelpSGFContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            //services.Configure<AppSettings>(Configuration.GetSection("AppSettings"));

            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<DataAccess.HelpSGFContext>()
                .AddDefaultTokenProviders();

            services.Configure<IdentityOptions>(O =>
            {
                O.Password.RequireDigit = true;
                O.Password.RequiredLength = 8;
                O.Password.RequireNonAlphanumeric = false;
                O.Password.RequireUppercase = true;
                O.Password.RequireLowercase = false;
                O.Password.RequiredUniqueChars = 6;

                O.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(30);
                O.Lockout.MaxFailedAccessAttempts = 10;
                O.Lockout.AllowedForNewUsers = true;

                O.User.RequireUniqueEmail = true;
            });

            services.ConfigureApplicationCookie(O =>
            {
                O.Cookie.HttpOnly = true;
                O.Cookie.Expiration = TimeSpan.FromDays(150);
                O.LoginPath = "/Account/Login";
                O.LogoutPath = "/Account/Logout";
                O.AccessDeniedPath = "/Account/AccessDenied";
                O.SlidingExpiration = true;
            });

            // Add application services.
            services.AddTransient<IEmailSender, EmailSender>();

            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseBrowserLink();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
