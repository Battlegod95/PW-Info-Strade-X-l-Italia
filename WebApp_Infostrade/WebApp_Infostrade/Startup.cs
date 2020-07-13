using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using WebApp_infostrade.Data;
using WebApp_Infostrade.Hubs;

namespace WebApp_Infostrade
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
            services.AddSignalR().AddAzureSignalR("Endpoint=https://signalrwebapp.service.signalr.net;AccessKey=3Bs4Twk5KhAi94wk4BXMZwK/3o60T7yINsfh81PfH/U=;Version=1.0;");
            services.AddControllersWithViews();

            services.AddScoped<ISensorRepository, SensorRepository>();

            /*services.AddCors(options => options.AddPolicy("CorsPolicy",
            builder =>
            {
                builder.AllowAnyMethod().AllowAnyHeader()
                       .WithOrigins("http://localhost:44336")
                       .AllowCredentials();
            }));*/
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
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

            app.UseFileServer();
            app.UseRouting();

            app.UseAuthorization();
            app.UseCors("CorsPolicy");

            app.UseEndpoints(endpoints =>
            {
                //endpoints.MapHub<SignalrHub>("/signalrwebapp");
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
            app.UseEndpoints(routes =>
            {
                routes.MapHub<SignalrHub>("/signalrwebapp");
            });
            /*app.UseAzureSignalR(routes =>
            {
                routes.MapHub<SignalrHub>("/signalrwebapp");
            });*/
        }
    }
}
