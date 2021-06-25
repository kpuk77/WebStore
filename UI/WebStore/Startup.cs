using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using System;
using Microsoft.AspNetCore.Identity;
using WebStore.DAL.Context;
using WebStore.Domain.Entities.Identity;
using WebStore.Interfaces.Services;
using WebStore.Services.Data;
using WebStore.Services.InCookies;
using WebStore.Services.InMemory;
using WebStore.Services.InSQL;

namespace WebStore
{
    public class Startup
    {
        private IConfiguration _Configuration { get; }

        public Startup(IConfiguration configuration) => _Configuration = configuration;

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddRazorPages().AddRazorRuntimeCompilation();
            services.AddControllersWithViews();

            var dbSource = _Configuration["DBSource"];
            switch (dbSource)
            {
                case "SQLite":
                    services.AddDbContext<WebStoreDB>(opt => opt.UseSqlite(_Configuration.GetConnectionString("SQLite"),
                        o => o.MigrationsAssembly("WebStore.DAL.Sqlite")));
                    break;
                case "MSSqlServer":
                    services.AddDbContext<WebStoreDB>(opt =>
                        opt.UseSqlServer(_Configuration.GetConnectionString("MSSqlServer")));
                    break;
            }

            services.AddTransient<IEmployeesData, InMemoryEmployeesData>();
            services.AddIdentity<User, Role>()
                .AddEntityFrameworkStores<WebStoreDB>()
                .AddDefaultTokenProviders();

            services.AddScoped<ICartService, InCookiesCartService>();

            services.Configure<IdentityOptions>(opt =>
            {
#if DEBUG
                opt.Password.RequireDigit = false;
                opt.Password.RequireLowercase = false;
                opt.Password.RequireUppercase = false;
                opt.Password.RequireNonAlphanumeric = false;
                opt.Password.RequiredLength = 3;
                opt.Password.RequiredUniqueChars = 3;
#endif

                opt.User.RequireUniqueEmail = false;
                opt.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";

                opt.Lockout.AllowedForNewUsers = false;
                opt.Lockout.MaxFailedAccessAttempts = 5;
                opt.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(15);
            });

            services.ConfigureApplicationCookie(opt =>
            {
                opt.Cookie.Name = "WebStore38r";
                opt.Cookie.HttpOnly = true;

                opt.ExpireTimeSpan = TimeSpan.FromDays(7);

                opt.LoginPath = "/Account/Login";
                opt.LogoutPath = "/Account/Logout";
                opt.AccessDeniedPath = "/Account/AccessDenied";

                opt.SlidingExpiration = true;
            });

            if (_Configuration["DataSource"].ToLower() == "db")
            {
                services.AddTransient<WebStoreDBInitializer>();
                services.AddScoped<IOrderService, SqlOrderData>();
                services.AddScoped<IProductData, SqlProductData>();
            }
            else if (_Configuration["DataSource"].ToLower() == "memory")
                services.AddSingleton<IProductData, InMemoryProductData>();
            else
                throw new Exception("�� ������ �������� ������.");
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IServiceProvider services)
        {
            if (_Configuration["DataSource"].ToLower() == "db")
                using (var scope = services.CreateScope())
                    scope.ServiceProvider.GetRequiredService<WebStoreDBInitializer>().Initialize();

            if (env.IsDevelopment())
                app.UseDeveloperExceptionPage();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseStaticFiles();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "areas",
                    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}"
                );

                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}"
                    );

                endpoints.MapControllerRoute(
                    name: "employees",
                    pattern: "{controller=Employees}/{action=Index}/{id?}"
                    );
            });
        }
    }
}