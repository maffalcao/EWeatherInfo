using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using StackExchange.Redis;
using Infra.Interfaces;
using Infra.Clients;
using Infra.Helpers;
using Infra.Repositories;
using Domain.Interfaces;
using Domain.Services;
using CrossCutting.Settings;
using Application.Filters;

namespace Application
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

            services.AddControllers(options =>
            {
                options.Filters.Add(typeof(ExceptionFilter));
            });

            services.AddApiVersioning(o =>
            {
                o.ReportApiVersions = true;
                o.AssumeDefaultVersionWhenUnspecified = true;
                o.DefaultApiVersion = new ApiVersion(1, 0);
            });

            services.AddScoped<IMeasurementRepository, MeasurementRepository>();
            services.AddScoped<IDeviceRepository, DeviceRepository>();
            services.AddScoped<IMeasurementService, MeasurementService>();
            services.AddScoped<IIO, IO>();
            services.AddScoped<IRedisClient, RedisClient>();

            ConnectionMultiplexer cm = ConnectionMultiplexer.Connect(Configuration.GetValue<string>("CacheSettings:ConnectionString"));
            services.AddSingleton<IConnectionMultiplexer>(cm);

            services.Configure<AppSettings>(options => Configuration.GetSection("AppSettings").Bind(options));

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Application", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Application v1"));
            }

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
