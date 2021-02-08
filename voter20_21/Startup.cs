using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using System.IO;
using voter20_21.Models;
using voter20_21.Services;

namespace voter20_21
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

            DbType dbType = Configuration.GetValue<DbType>("DbType");

            //DbInitializer.Initialize(context);
            //VoterService service = new VoterService(context);

            switch (dbType)
            {
                // Need Microsoft.EntityFrameworkCore.SqlServer package for this
                case DbType.SqlServer:
                    services.AddDbContext<voterContext>(options =>
                        options.UseSqlServer(Configuration.GetConnectionString("SqlServerConnection"))
                        );
                    break;
                // Need Microsoft.EntityFrameworkCore.Sqlite package for this
                // Using Microsoft.EntityFrameworkCore.Sqlite.Core causes error with update-database
                case DbType.Sqlite:
                    services.AddDbContext<voterContext>(o => 
                        o.UseSqlite(Configuration.GetConnectionString("SqliteConnection"))
                        );
                    break;
            }
            services.AddTransient<VoterService>();
            services.AddTransient<AccountService>();

            services.AddSingleton<ApplicationState>();
            //services.AddSingleton<ApplicationState>();
            // Dependency injection a IHttpContextAccessor-hoz
            services.AddHttpContextAccessor();

            services.AddControllersWithViews();

            // Munkamenetkezelés beállítása
            //services.AddDistributedMemoryCache();
            services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(15); // max. 15 percig él a munkamenet
            });
        }
    

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IServiceProvider provider)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();
            app.UseSession();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
            ///TODO:
            DbInitializer.Initialize(provider, Configuration.GetValue<string>("IamgeStore"));
        }
    }
}
