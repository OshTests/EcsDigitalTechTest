using AutoMapper;
using EcsDigitalApi.Entities;
using EcsDigitalApi.Repositories;
using EcsDigitalApi.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace EcsDigitalApi
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
            services.AddScoped<IMapper, Mapper>(serviceProvider => new Mapper(new MapperConfiguration(CreateMaps)));
            services.AddScoped<ICarService, CarService>();
            services.AddScoped<ICarRepository, CarRepository>();

            //Using the MVC built in dependency injection (scoped by default).
            services.AddDbContext<CarsContext>(options => options.UseInMemoryDatabase("CarsContext"));
        }

        private void CreateMaps(IMapperConfigurationExpression config)
        {
            config.CreateMap<Car, ApiModels.Car>();
            config.CreateMap<ApiModels.Car, Car>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            using (var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
            {
                var context = serviceScope.ServiceProvider.GetRequiredService<CarsContext>();
                context.SeedInMemory().Wait();
            }

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
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
