using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using AuditTrail;
using AuditTrail.Model;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using System;

namespace AspNetCoreElasticsearchNestAuditTrail
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }
        public void ConfigureServices(IServiceCollection services)
        {
            var indexPerMonth = false;
            var amountOfPreviousIndicesUsedInAlias = 3;
            services.AddAuditTrail<CustomAuditTrailLog>(options => 
                options.UseSettings(indexPerMonth, amountOfPreviousIndicesUsedInAlias)
            );
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = $"接口文档",
                    Description = $"HTTP API ",
                    Contact = new OpenApiContact { Name = "懒得勤快", Email = "admin@masuit.com", Url = new Uri("https://masuit.coom") },
                    License = new OpenApiLicense { Name = "懒得勤快", Url = new Uri("https://masuit.com") }
                });
                c.IncludeXmlComments(AppContext.BaseDirectory + "AspNetCoreElasticsearchNestAuditTrail.xml");
            }); //配置swagger
            services.AddControllers();
            services.AddControllersWithViews()
                .AddNewtonsoftJson();
          
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
            }


            app.UseDefaultFiles();
            app.UseStaticFiles();

            app.UseRouting();
            app.UseSwagger().UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint($"/swagger/v1/swagger.json", "懒得勤快的博客，搜索引擎测试");
            }); //配置swagger
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
