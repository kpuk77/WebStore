using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using System;

using WebStore.DAL.Context;
using WebStore.Data;
using WebStore.Services.InMemory;
using WebStore.Services.InSQL;
using WebStore.Services.Interfaces;

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

            services.AddDbContext<WebStoreDB>(opt =>
                opt.UseSqlServer(_Configuration.GetConnectionString("MSSqlServer")));

            services.AddTransient<IEmployeesData, InMemoryEmployeesData>();

            if (_Configuration["DataSource"].ToLower() == "db")
            {
                services.AddTransient<WebStoreDBInitializer>();
                services.AddScoped<IProductData, SqlProductData>();
            }
            else if (_Configuration["DataSource"].ToLower() == "memory")
                services.AddSingleton<IProductData, InMemoryProductData>();
            else
                throw new Exception("Не выбран источник данных.");
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IServiceProvider services)
        {
            if (_Configuration["DataSource"].ToLower() == "db")
                using (var scope = services.CreateScope())
                    scope.ServiceProvider.GetRequiredService<WebStoreDBInitializer>().Initialize();

            if (env.IsDevelopment())
                app.UseDeveloperExceptionPage();
            
            app.UseRouting();
            app.UseStaticFiles();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    "default",
                    "{controller=Home}/{action=Index}/{id?}");

                endpoints.MapControllerRoute(
                    "employees",
                    "{controller=Employees}/{action=Index}/{id?}");
            });
        }
    }
}