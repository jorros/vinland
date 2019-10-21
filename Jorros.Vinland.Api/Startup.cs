using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Jorros.Vinland.Api.Configuration;
using Jorros.Vinland.Data;
using Jorros.Vinland.Data.Repositories;
using Jorros.Vinland.OrderProcessing;
using Jorros.Vinland.OrderProcessing.Batch;
using Jorros.Vinland.OrderProcessing.MappingProfiles;
using Jorros.Vinland.Pricing;
using Jorros.Vinland.Pricing.FrenchWinery;
using Jorros.Vinland.WineProviders;
using Jorros.Vinland.WineProviders.FrenchWinery;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SpaServices.AngularCli;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Jorros.Vinland.Api
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

            services.AddTransient<IOrderService, BatchOrderService>();
            services.AddTransient<IWineProvider, FrenchWineryWineProvider>();
            services.AddTransient<IPricingService, FrenchWineryPricingService>();

            services.Configure<BatchOrderSettings>(Configuration.GetSection("BatchOrder"));
            services.Configure<FrenchWineryPricingSettings>(Configuration.GetSection("FrenchWineryPricing"));

            services.AddDbContext<VinlandContext>(opt => opt.UseInMemoryDatabase("vinland"));
            services.AddTransient<IOrderRepository, OrderRepository>();
            services.AddTransient<IBottleRepository, BottleRepository>();

            services.AddAutoMapper(typeof(MappingConfiguration), typeof(OrderProfile));

            services.AddCors(options =>
            {
                options.AddDefaultPolicy(builder => builder
                        .AllowAnyOrigin()
                        .AllowAnyHeader()
                        .AllowAnyMethod());
            });

            services.AddSpaStaticFiles(configuration =>
            {
                configuration.RootPath = "ClientApp/dist";
            });

            services.AddLogging();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();
            app.UseSpaStaticFiles();

            app.UseAuthorization();

            app.UseCors();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.UseSpa(spa =>
            {
                spa.Options.SourcePath = "ClientApp";

                if (env.IsDevelopment())
                {
                    spa.UseAngularCliServer(npmScript: "start");
                }
            });
        }
    }
}
