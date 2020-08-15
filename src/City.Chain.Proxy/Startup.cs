using AspNetCore.Proxy;
using City.Chain.Proxy.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using System;

namespace City.Chain.Proxy
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
            services.AddControllers();

            services.AddProxies();

            services.AddSingleton<NodeService>();

            services.AddSwaggerGen(
                options =>
                {
                    string assemblyVersion = typeof(Startup).Assembly.GetName().Version.ToString();

                    options.SwaggerDoc("proxy",
                               new OpenApiInfo
                               {
                                   Title = "City Chain Proxy API",
                                   Version = assemblyVersion,
                                   Description = ".",
                                   Contact = new OpenApiContact
                                   {
                                       Name = "City Chain",
                                       Url = new Uri("https://www.city-chain.org/")
                                   }
                               });

                    //if (File.Exists(XmlCommentsFilePath))
                    //{
                    //    options.IncludeXmlComments(XmlCommentsFilePath);
                    //}

                    options.DescribeAllEnumsAsStrings();

                    options.DescribeStringEnumsInCamelCase();

                    options.EnableAnnotations();
                });

            services.AddSwaggerGenNewtonsoftSupport(); // explicit opt-in - needs to be placed after AddSwaggerGen()
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseSwagger(c =>
            {
                c.RouteTemplate = "docs/{documentName}/openapi.json";
            });

            app.UseSwaggerUI(c =>
            {
                c.RoutePrefix = "docs";
                c.SwaggerEndpoint("/docs/proxy/openapi.json", "City Chain Proxy API");
            });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
