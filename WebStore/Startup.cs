using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace WebStore
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration Configuration) => this.Configuration = Configuration;

        public void ConfigureServices(IServiceCollection services) => 
            services.AddControllersWithViews();

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
                app.UseDeveloperExceptionPage();
            //app.UseExceptionHandler("/Errors");

            app.UseRouting();

            app.UseStaticFiles();

            app.UseEndpoints(endpoints =>
            {
                //endpoints.MapDefaultControllerRoute();
                endpoints.MapControllerRoute(
                    "default",
                    "{controller=Home}/{action=Index}/{id?}");

                endpoints.MapControllerRoute(
                    "employees",
                    "{controller=Employees}/{action=Index}/{id?}");

                //endpoints.MapControllerRoute(
                //    "errors",
                //    "{controller=Errors}/{action=Index}/{id!})");
            });
        }
    }
}