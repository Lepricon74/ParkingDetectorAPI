using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using ParkingDetectorAPI.DBRepository;
using Microsoft.OpenApi.Models;
//Нужно при использовании обратного прокси-сервера
using Microsoft.AspNetCore.HttpOverrides;

namespace ParkingDetectorAPI
{
    public class Startup
    {

        public IConfiguration Configuration;
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            //string connection = Configuration.GetConnectionString("PostgreSQLConnection");
            string connection = Configuration.GetConnectionString("PostgreSQLExternalConnection");
            services.AddDbContext<ParkingDetectorAPIContext>(options => options.UseNpgsql(connection));


            //services.AddMvc(options => options.EnableEndpointRouting = false);
            services.AddMvc();
            services.AddControllersWithViews();
            services.AddControllers();

            // Register the Swagger generator, defining 1 or more Swagger documents
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "Parking Detector API",
                    Description = "ASP.NET Core Parking Detector API. Here you can see the available routes and response formats for this API",                    
                });
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseForwardedHeaders(new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
            });

            app.UseStaticFiles();

            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();
            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.)
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "ParkingDetector API V1");
            });

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}");
                endpoints.MapControllers();
            });
          
        }
    }
}
